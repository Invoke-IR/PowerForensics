using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Management.Automation;

namespace InvokeIR.PowerForensics
{

    public static class Main
    {

        #region PInvoke

        //function import
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetFileInformationByHandle(
            Microsoft.Win32.SafeHandles.SafeFileHandle hFile,
            out BY_HANDLE_FILE_INFORMATION lpFileInformation
        );

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeviceIoControl(
                SafeFileHandle hDevice,
                uint dwIoControlCode,
                [MarshalAs(UnmanagedType.AsAny)]
                [In] object InBuffer,
                int nInBufferSize,
                [MarshalAs(UnmanagedType.AsAny)]
                [Out] object OutBuffer,
                int nOutBufferSize,
                ref int lpBytesReturned,
                [In] IntPtr lpOverlapped);

        #endregion PInvoke


        #region privateMethods

        private static SafeFileHandle getHandle(string FileName)
        {

            // Get Handle to specified Volume/File/Directory
            SafeFileHandle hDrive = CreateFile(
                fileName: FileName,
                fileAccess: FileAccess.Read,
                fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                securityAttributes: IntPtr.Zero,
                creationDisposition: FileMode.Open,
                flags: 0x02000000, //with this also an enum can be used. (as described above as EFileAttributes)
                template: IntPtr.Zero);

            // Check if handle is valid
            if (hDrive.IsInvalid)
            {

                if(Marshal.GetLastWin32Error() != 32)
                {
                    
                    throw new IOException("Unable to access drive. Win32 Error Code " + Marshal.GetLastWin32Error());
                    //if get windows error code 5 this means access denied. You must try to run the program as admin privileges.

                }
                
            }

            return hDrive;

        }

        private static byte[] ReadDrive(string fileName, long offset, long sizeToRead)
        {

            // Get Handle to the specified Volume
            SafeFileHandle hDrive = getHandle(fileName);

            // Bytes must be read by sector
            if ((sizeToRead < 1)) throw new System.ArgumentException("Size parameter cannot be null or 0 or less than 0!");
            if (((sizeToRead % 512) != 0)) throw new System.ArgumentException("Size parameter must be divisible by 512");
            if (((offset % 512) != 0)) throw new System.ArgumentException("Offset parameter must be divisible by 512");

            // Create a FileStream to read from the specified handle
            FileStream diskStreamToRead = new FileStream(hDrive, FileAccess.Read);
            // Set offset to begin reading from the drive
            diskStreamToRead.Position = offset;
            // Create a byte array to read into
            byte[] buf = new byte[sizeToRead];
            // Read buf.Length bytes (sizeToRead) from offset 
            diskStreamToRead.Read(buf, 0, buf.Length);
            // Close handle
            try { hDrive.Close(); }
            catch { }
            // Close FileStream
            try { diskStreamToRead.Close(); }
            catch { }

            return buf;

        }

        private static bool checkNTFS(NTFS_BPB ntfsHeader)
        {
            byte[] ntfsSig = new byte[] { 0x4E, 0x54, 0x46, 0x53, 0x20, 0x20, 0x20, 0x20 };
            for (var i = 0; i + ntfsHeader.Signature.Length < ntfsSig.Length; i++)
            {
                var allSame = true;
                for (var j = 0; j < ntfsHeader.Signature.Length; j++)
                {
                    if (ntfsSig[i + j] != ntfsHeader.Signature[j])
                    {
                        allSame = false;
                        break;
                    }
                }

                if (allSame)
                {
                    return true;
                }
            }

            return false;

        }

        private static bool checkMFTRecord(FILE_RECORD_HEADER mftRecordHeader)
        {
            return mftRecordHeader.Magic == 1162627398;
        }

        private static byte[] getMFTRecordBytes(string volume, uint inode)
        {

            // Gather Data about Volume
            NTFSVolumeData volData = getVolumeDataInformation(volume);

            // Calculate byte offset to the Master File Table (MFT)
            long mftOffset = (volData.BytesPerCluster * volData.MFTStartCluster);

            // Determine offset to specified MFT Record
            long offsetMFTRecord = (inode * volData.BytesPerMFTRecord) + mftOffset;

            // Read bytes belonging to specified MFT Record and store in byte array
            return ReadDrive(volume, offsetMFTRecord, volData.BytesPerMFTRecord);

        }

        private static byte[] getNonResBytes(string volume, NonResident nonResAttr)
        {
            List<byte> DataBytes = new List<byte>();

            for (int i = 0; i < nonResAttr.StartCluster.Length; i++)
            {
                long offset = (long)nonResAttr.StartCluster[i] * 4096;
                long length = ((long)nonResAttr.EndCluster[i] - (long)nonResAttr.StartCluster[i]) * 4096;
                DataBytes.AddRange(ReadDrive(volume, offset, length));
            }

            byte[] DataArray = DataBytes.ToArray();
            Array.Resize(ref DataArray, (int)nonResAttr.RealSize);

            return DataArray;
        }

        private static NonResident getNonResAttribute(byte[] MFTRecordBytes, ATTR_HEADER_COMMON commonHeader, string AttrName)
        {
            ATTR_HEADER_NON_RESIDENT nonResAttrHeader = new ATTR_HEADER_NON_RESIDENT(commonHeader, MFTRecordBytes.Skip(16).Take(48).ToArray());

            int offset = 0;
            int DataRunStart = nonResAttrHeader.DataRunOffset;
            int DataRunSize = (int)commonHeader.TotalSize - nonResAttrHeader.DataRunOffset;
            byte[] DataRunBytes = MFTRecordBytes.Skip(DataRunStart).Take(DataRunSize).ToArray();

            int DataRunLengthByteCount = DataRunBytes[offset] & 0x0F;
            int DataRunOffsetByteCount = ((DataRunBytes[offset] & 0xF0) >> 4);

            int i = 0;
            ulong startCluster = 0;
            ulong[] startClusterArray = new ulong[1];
            ulong[] endClusterArray = new ulong[1];

            while ((offset < DataRunSize - 1) && (DataRunLengthByteCount != 0))
            {

                byte[] DataRunLengthBytes = DataRunBytes.Skip(offset + 1).Take(DataRunLengthByteCount).ToArray();
                Array.Resize(ref DataRunLengthBytes, 8);
                byte[] DataRunOffsetBytes = DataRunBytes.Skip((offset + 1 + DataRunLengthByteCount)).Take(DataRunOffsetByteCount).ToArray();
                Array.Resize(ref DataRunOffsetBytes, 8);

                ulong DataRunLength = BitConverter.ToUInt64(DataRunLengthBytes, 0);
                ulong DataRunOffset = BitConverter.ToUInt64(DataRunOffsetBytes, 0);

                Array.Resize(ref startClusterArray, (i + 1));
                Array.Resize(ref endClusterArray, (i + 1));

                startCluster += DataRunOffset;
                startClusterArray[i] = startCluster;
                endClusterArray[i] = (startCluster + DataRunLength);

                offset = offset + 1 + DataRunLengthByteCount + DataRunOffsetByteCount;

                DataRunLengthByteCount = DataRunBytes[offset] & 0x0F;
                DataRunOffsetByteCount = ((DataRunBytes[offset] & 0xF0) >> 4);

                i++;

            }

            NonResident nonResAttr = new NonResident(commonHeader, AttrName, nonResAttrHeader, startClusterArray, endClusterArray);
            return nonResAttr;
        }

        private static AttributeReturn getAttribute(byte[] Bytes, int offsetToATTR)
        {
            ATTR_HEADER_COMMON commonAttributeHeader = new ATTR_HEADER_COMMON(Bytes.Skip(offsetToATTR).Take(16).ToArray());

            byte[] AttrBytes = Bytes.Skip(offsetToATTR).Take((int)commonAttributeHeader.TotalSize).ToArray();
            byte[] NameBytes = AttrBytes.Skip(commonAttributeHeader.NameOffset).Take(commonAttributeHeader.NameLength * 2).ToArray();
            string AttrName = Encoding.Unicode.GetString(NameBytes);

            int offset = offsetToATTR + (int)commonAttributeHeader.TotalSize;
            AttributeReturn attrReturn = null;

            if (commonAttributeHeader.NonResident)
            {
                NonResident NonResAttr = getNonResAttribute(AttrBytes, commonAttributeHeader, AttrName);
                attrReturn = new AttributeReturn(NonResAttr, offset);
            }
            else
            {
                ATTR_HEADER_RESIDENT AttrHeaderResident = new ATTR_HEADER_RESIDENT(commonAttributeHeader, AttrBytes.Skip(16).Take(8).ToArray());

                switch (commonAttributeHeader.ATTRType)
                {

                    case (Int32)ATTR_TYPE.STANDARD_INFORMATION:
                        ATTR_STANDARD_INFORMATION stdInfo = new ATTR_STANDARD_INFORMATION(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray(), (int)AttrHeaderResident.AttrSize);

                        #region stdInfoFlags
                        string permissionFlags = null;
                        if (stdInfo.Permission != 0)
                        {
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.READONLY) == (uint)ATTR_STDINFO_PERMISSION.READONLY)
                            {
                                permissionFlags += "READONLY, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.HIDDEN) == (uint)ATTR_STDINFO_PERMISSION.HIDDEN)
                            {
                                permissionFlags += "HIDDEN, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.SYSTEM) == (uint)ATTR_STDINFO_PERMISSION.SYSTEM)
                            {
                                permissionFlags += "SYSTEM, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.ARCHIVE) == (uint)ATTR_STDINFO_PERMISSION.ARCHIVE)
                            {
                                permissionFlags += "ARCHIVE, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.DEVICE) == (uint)ATTR_STDINFO_PERMISSION.DEVICE)
                            {
                                permissionFlags += "DEVICE, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.NORMAL) == (uint)ATTR_STDINFO_PERMISSION.NORMAL)
                            {
                                permissionFlags += "NORMAL, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.TEMP) == (uint)ATTR_STDINFO_PERMISSION.TEMP)
                            {
                                permissionFlags += "TEMP, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.SPARSE) == (uint)ATTR_STDINFO_PERMISSION.SPARSE)
                            {
                                permissionFlags += "SPARSE, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.REPARSE) == (uint)ATTR_STDINFO_PERMISSION.REPARSE)
                            {
                                permissionFlags += "REPARSE, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.COMPRESSED) == (uint)ATTR_STDINFO_PERMISSION.COMPRESSED)
                            {
                                permissionFlags += "COMPRESSED, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.OFFLINE) == (uint)ATTR_STDINFO_PERMISSION.OFFLINE)
                            {
                                permissionFlags += "OFFLINE, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.NCI) == (uint)ATTR_STDINFO_PERMISSION.NCI)
                            {
                                permissionFlags += "NCI, ";
                            }
                            if ((stdInfo.Permission & (uint)ATTR_STDINFO_PERMISSION.ENCRYPTED) == (uint)ATTR_STDINFO_PERMISSION.ENCRYPTED)
                            {
                                permissionFlags += "ENCRYPTED, ";
                            }
                            permissionFlags = permissionFlags.TrimEnd(' ').TrimEnd(',');
                        }
                        #endregion stdInfoFlags

                        StandardInformation StdInfoAttr = new StandardInformation(commonAttributeHeader, AttrName, permissionFlags, stdInfo);
                        attrReturn = new AttributeReturn(StdInfoAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.ATTRIBUTE_LIST:
                        break;

                    case (Int32)ATTR_TYPE.FILE_NAME:
                        ATTR_FILE_NAME fileName = new ATTR_FILE_NAME(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
                        ulong ParentIndex = fileName.ParentRef & 0x000000000000FFFF;
                        FileName FileNameAttr = new FileName(commonAttributeHeader, AttrName, fileName, ParentIndex);
                        attrReturn = new AttributeReturn(FileNameAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.OBJECT_ID:
                        ATTR_OBJECT_ID objectId = new ATTR_OBJECT_ID(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
                        ObjectId ObjectIdAttr = new ObjectId(commonAttributeHeader, AttrName, objectId);
                        attrReturn = new AttributeReturn(ObjectIdAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.SECURITY_DESCRIPTOR:
                        break;

                    case (Int32)ATTR_TYPE.VOLUME_NAME:
                        string vName = Encoding.Unicode.GetString(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
                        VolumeName VolumeNameAttr = new VolumeName(commonAttributeHeader, AttrName, vName);
                        attrReturn = new AttributeReturn(VolumeNameAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.VOLUME_INFORMATION:
                        ATTR_VOLUME_INFORMATION volInfo = new ATTR_VOLUME_INFORMATION(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
                        Int16 flags = BitConverter.ToInt16(volInfo.Flags, 0);

                        #region volInfoFlags

                        string volumeFlags = null;
                        if (flags != 0)
                        {
                            if ((flags & (int)ATTR_VOLINFO.FLAG_DIRTY) == (int)ATTR_VOLINFO.FLAG_DIRTY)
                            {
                                volumeFlags += "Dirty, ";
                            }
                            if ((flags & (int)ATTR_VOLINFO.FLAG_RLF) == (int)ATTR_VOLINFO.FLAG_RLF)
                            {
                                volumeFlags += "Resize Logfile, ";
                            }
                            if ((flags & (int)ATTR_VOLINFO.FLAG_UOM) == (int)ATTR_VOLINFO.FLAG_UOM)
                            {
                                volumeFlags += "Upgrade on Mount, ";
                            }
                            if ((flags & (int)ATTR_VOLINFO.FLAG_MONT) == (int)ATTR_VOLINFO.FLAG_MONT)
                            {
                                volumeFlags += "Mounted on NT4, ";
                            }
                            if ((flags & (int)ATTR_VOLINFO.FLAG_DUSN) == (int)ATTR_VOLINFO.FLAG_DUSN)
                            {
                                volumeFlags += "Delete USN Underway, ";
                            }
                            if ((flags & (int)ATTR_VOLINFO.FLAG_ROI) == (int)ATTR_VOLINFO.FLAG_ROI)
                            {
                                volumeFlags += "Repair Object Ids, ";
                            }
                            if ((flags & (int)ATTR_VOLINFO.FLAG_MBC) == (int)ATTR_VOLINFO.FLAG_MBC)
                            {
                                volumeFlags += "Modified By ChkDisk, ";
                            }
                            volumeFlags = volumeFlags.TrimEnd(' ').TrimEnd(',');
                        }

                        #endregion volInfoFlags

                        VolumeInformation VolInfoAttr = new VolumeInformation(commonAttributeHeader, AttrName, volInfo, volumeFlags);
                        attrReturn = new AttributeReturn(VolInfoAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.DATA:
                        Data DataAttr = new Data(commonAttributeHeader, AttrName, AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
                        attrReturn = new AttributeReturn(DataAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.INDEX_ROOT:
                        ATTR_INDEX_ROOT indxRoot = new ATTR_INDEX_ROOT(AttrBytes.Skip(16 + 8).Take((int)AttrHeaderResident.AttrSize).ToArray());
                        IndexRoot indxRootAttr = new IndexRoot(commonAttributeHeader, AttrName, indxRoot, "test");
                        attrReturn = new AttributeReturn(indxRootAttr, offset);
                        break;

                    case (Int32)ATTR_TYPE.INDEX_ALLOCATION:
                        break;

                    case (Int32)ATTR_TYPE.BITMAP:
                        break;

                    case (Int32)ATTR_TYPE.REPARSE_POINT:
                        break;

                    case (Int32)ATTR_TYPE.EA_INFORMATION:
                        break;

                    case (Int32)ATTR_TYPE.EA:
                        break;

                    case (Int32)ATTR_TYPE.LOGGED_UTILITY_STREAM:
                        break;

                    default:
                        break;

                }

            }

            if (attrReturn == null)
            {
                Attribute attr = new Attribute(commonAttributeHeader, AttrName);
                attrReturn = new AttributeReturn(attr, offset);
            }

            return attrReturn;
        }

        private static List<IndexEntry> getIndexEntry(string volume, uint inode)
        {

            FileRecord fileRecord = getFileRecord(volume, inode);

            NonResident INDX = null;

            foreach(Attribute attr in fileRecord.Attribute)
            {
                if (attr.Name == "INDEX_ALLOCATION")
                {
                    if (attr.NonResident)
                    {
                        INDX = (NonResident)attr;
                    }
                }
            }

            byte[] nonResBytes = getNonResBytes(volume, INDX);

            List<IndexEntry> indxEntryList = new List<IndexEntry>();

            for (int offset = 0; offset < nonResBytes.Length; offset += 4096)
            {

                byte[] indxBytes = nonResBytes.Skip(offset).Take(4096).ToArray();

                INDEX_BLOCK indxBlock = new INDEX_BLOCK(indxBytes.Take(40).ToArray());

                byte[] IndexAllocEntryBytes = indxBytes.Skip(64).ToArray();
                
                int offsetIndx = 0;
                int offsetIndxPrev = 1;

                while ((offsetIndx < IndexAllocEntryBytes.Length) && (offsetIndx != offsetIndxPrev))
                {

                    INDEX_ENTRY indxEntryStruct = new INDEX_ENTRY(IndexAllocEntryBytes.Skip(offsetIndx).ToArray());

                    offsetIndxPrev = offsetIndx;
                    offsetIndx += indxEntryStruct.Size;
                    if (indxEntryStruct.Stream.Length > 66)
                    {
                        //Console.WriteLine("Length: {0}", indxEntryStruct.Stream.Length);
                        ATTR_FILE_NAME fileNameStruct = new ATTR_FILE_NAME(indxEntryStruct.Stream);

                        #region indxFlags

                        string indxFlags = null;
                        if (indxEntryStruct.Flags != 0)
                        {
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.SUBNODE) == (int)INDEX_ENTRY_FLAG.SUBNODE)
                            {
                                indxFlags += "Subnode, ";
                            }
                            if ((indxEntryStruct.Flags & (int)INDEX_ENTRY_FLAG.LAST) == (int)INDEX_ENTRY_FLAG.LAST)
                            {
                                indxFlags += "Last Entry, ";
                            }
                            indxFlags = indxFlags.TrimEnd(' ').TrimEnd(',');
                        }

                        #endregion indxFlags

                        string Name = System.Text.Encoding.Unicode.GetString(fileNameStruct.Name);
                        IndexEntry indxEntry = new IndexEntry(indxEntryStruct, indxFlags, Name);
                        indxEntryList.Add(indxEntry);

                        //Console.WriteLine("Capacity: {0}", indxEntryList.Capacity);
                        //Console.WriteLine("Count: {0}", indxEntryList.Count);
                    }

                }

            }

            return indxEntryList; 
        }

        #endregion privateMethods


        #region publicMethods

        // mmls
        public static void getPartitionTable(string HardDiskName)
        {
            //Needs Work
        }

        // fsstat
        public static NTFSVolumeData getVolumeDataInformation(string volume)
        {

            // Get Handle to the specified Volume
            SafeFileHandle hDrive = getHandle(volume);

            // Create a byte array the size of the NTFS_VOLUME_DATA_BUFFER struct
            byte[] ntfsVolData = new byte[96];
            // Instatiate an integer to accept the amount of bytes read
            int buf = new int();

            // Return the NTFS_VOLUME_DATA_BUFFER struct
            var status = DeviceIoControl(
                hDevice: hDrive,
                dwIoControlCode: (uint)0x00090064,
                InBuffer: null,
                nInBufferSize: 0,
                OutBuffer: ntfsVolData,
                nOutBufferSize: ntfsVolData.Length,
                lpBytesReturned: ref buf,
                lpOverlapped: IntPtr.Zero);

            // Close handle
            try { hDrive.Close(); }
            catch { }

            // Return the NTFSVolumeData Object
            return new NTFSVolumeData(new NTFS_VOLUME_DATA_BUFFER(ntfsVolData));

        }

        public static void dd(string inFile, string outFile, long offset, int blockSize, int count)
        {

            long sizeToRead = blockSize * count;

            // Read sizeToRead bytes from the Volume
            byte[] buffer = ReadDrive(inFile, offset, sizeToRead);

            // Open file for reading
            System.IO.FileStream _FileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(buffer, 0, buffer.Length);
            // close file stream
            _FileStream.Close();

        }

        // istat
        public static FileRecord getFileRecord(string volume, uint inode)
        {

            byte[] MFTRecordBytes = getMFTRecordBytes(volume, inode);

            // Instantiate a FILE_RECORD_HEADER struct from raw MFT Record bytes
            FILE_RECORD_HEADER RecordHeader = new FILE_RECORD_HEADER(MFTRecordBytes);

            // Check MFT Signature (FILE) to ensure bytes actually represent an MFT Record
            if (checkMFTRecord(RecordHeader))
            {

                // Unmask Header Flags
                #region HeaderFlags

                string flagAttr = null;
                if (RecordHeader.Flags != 0)
                {
                    if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.INUSE) == (ushort)FILE_RECORD_FLAG.INUSE)
                    {
                        flagAttr += "InUse, ";
                    }
                    if ((RecordHeader.Flags & (ushort)FILE_RECORD_FLAG.DIR) == (ushort)FILE_RECORD_FLAG.DIR)
                    {
                        flagAttr += "Directory, ";
                    }
                    flagAttr = flagAttr.TrimEnd(' ').TrimEnd(',');
                }

                #endregion HeaderFlags

                List<Attribute> AttributeList = new List<Attribute>();
                int offsetToATTR = RecordHeader.OffsetOfAttr;

                while (offsetToATTR < (RecordHeader.RealSize - 8))
                {
                    AttributeReturn attrReturn = getAttribute(MFTRecordBytes, offsetToATTR);
                    offsetToATTR = attrReturn.StartByte;
                    AttributeList.Add(attrReturn.Attribute);
                }

                Attribute[] AttributeArray = AttributeList.ToArray();

                // Return FileRecord object
                return new FileRecord(RecordHeader.RecordNo, RecordHeader.SeqNo, RecordHeader.LSN, RecordHeader.Hardlinks, flagAttr, AttributeArray);

            }

            else
            {
                Console.WriteLine("Fail");
                return null;
            }

        }

        // icat
        public static byte[] getFile(string fileName)
        {
            string volumeLetter = fileName.Split('\\')[0];
            uint inode = getInode(fileName);
            string volume = @"\\.\" + volumeLetter;
            return getFile(volume, inode);
        }

        public static byte[] getFile(string volume, uint inode)
        {

            // Get the FileRecord (MFT Record Entry) for the given inode on the specified volume
            FileRecord MFTRecord = getFileRecord(volume, inode);

            if (!(MFTRecord.Flags.Contains("Directory")))
            {

                foreach (Attribute attr in MFTRecord.Attribute)
                {

                    if (attr.Name == "DATA")
                    {

                        if (attr.NonResident == true)
                        {

                            NonResident nonResAttr = (NonResident)attr;

                            return getNonResBytes(volume, nonResAttr);

                        }
                        
                        else
                        {

                            Data dataAttr = (Data)attr;
                            return dataAttr.RawData;
                        
                        }

                    }

                }

            }

            return null;
        
        }

        public static byte[] getFile(string volume, uint inode, uint attributeNumber)
        {
            return null;
        }
        
        // Return the MFT Record Index Number (Inode) for the specified File or Directory
        public static uint getInode(string FileName)
        {

            // Create a handle to the specified file or directory
            SafeFileHandle hFile = getHandle(FileName);

            // Check to see if file handle is valid
            if (hFile.IsInvalid)
            {

                // Throw error if file and directory handles are invalid
                //throw new IOException("Unable to access drive. Win32 Error Code " + Marshal.GetLastWin32Error());
                //if get windows error code 5 this means access denied. You must try to run the program as admin privileges.
                string[] directoryArray = FileName.Split('\\');
                string directory = null;
                for (int i = 0; i < (directoryArray.Length - 1); i++)
                {
                    directory += directoryArray[i];
                    directory += "\\";
                }

                uint inode = getInode(directory);

                string volume = @"\\.\" + directoryArray[0];

                List<IndexEntry> indxArray = getIndexEntry(volume, inode);

                foreach(IndexEntry indxEntry in indxArray)
                {
                    if(indxEntry.Name == directoryArray[(directoryArray.Length - 1)])
                    {
                        return (uint)indxEntry.FileIndex;
                    }
                }

            }

            BY_HANDLE_FILE_INFORMATION fileInfo = new BY_HANDLE_FILE_INFORMATION();
            bool Success = GetFileInformationByHandle(
                hFile: hFile,
                lpFileInformation: out fileInfo);

            // Combine two 32 bit unsigned integers into one 64 bit unsigned integer
            ulong FileIndex = fileInfo.nFileIndexHigh;
            FileIndex = FileIndex << 32;
            FileIndex = FileIndex + fileInfo.nFileIndexLow;
            // Unmask relevent bytes for MFT Index Number
            ulong Index = FileIndex & 0x0000FFFFFFFFFFFF;

            try { hFile.Close(); }
            catch { }

            return (uint)Index;

        }

        #endregion publicMethods

    }

}


namespace InvokeIR.PowerForensics.TSK
{
    #region GetFSStatCommand
    /// <summary> 
    /// This class implements the Get-FSStat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "FSStat", SupportsShouldProcess = true)]
    public class GetFSStatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the NTFSVolumeData object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string VolumeName
        {
            get { return volumeName; }
            set { volumeName = value; }
        }
        private string volumeName;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a NTFSVolumeData objects that
        /// corresponds to the VolumeName that is specified.
        /// </summary> 
    
        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");
            
            if (lettersOnly.IsMatch(volumeName))
            {
             
                volumeName = @"\\.\" + volumeName + ":";
            
            }

            WriteDebug("VolumeName: " + volumeName);
            
            NTFSVolumeData volumeData = InvokeIR.PowerForensics.Main.getVolumeDataInformation(volumeName);
            WriteObject(volumeData);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetFSstatCommand

    #region GetIStatCommand
    /// <summary> 
    /// This class implements the Get-IStat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "IStat", DefaultParameterSetName = "One", SupportsShouldProcess = true)]
    public class GetIStatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// This parameter provides the MFTIndexNumber for the 
        /// FileRecord object that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, ParameterSetName = "One")]
        public uint IndexNumber
        {
            get { return indexNumber; }
            set { indexNumber = value; }
        }
        private uint indexNumber;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, ParameterSetName = "Two")]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a NTFSVolumeData objects that
        /// corresponds to the VolumeName that is specified.
        /// </summary> 

        protected override void ProcessRecord()
        {

            if (this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {

                indexNumber = InvokeIR.PowerForensics.Main.getInode(filePath); 

            }

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";

            }

            WriteDebug("VolumeName: " + volume);

            FileRecord MFTRecord = InvokeIR.PowerForensics.Main.getFileRecord(volume, indexNumber);
            WriteObject(MFTRecord);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetIStatCommand
   
}

namespace InvokeIR.PowerForensics.DD
{

    #region ExportPowerDDCommand
    /// <summary> 
    /// This class implements the Export-PowerDD cmdlet. 
    /// </summary> 

    [Cmdlet("Export", "PowerDD", SupportsShouldProcess = true)]
    public class ExportPowerDDCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the File or Volume
        /// that should be read from (Ex. \\.\C: or C).
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string InFile
        {
            get { return inFile; }
            set { inFile = value; }
        }
        private string inFile;

        /// <summary> 
        /// This parameter provides the Name of the File
        /// that the read bytes should be output to.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string OutFile
        {
            get { return outFile; }
            set { outFile = value; }
        }
        private string outFile;

        /// <summary> 
        /// This parameter provides the Offset into the 
        /// specified InFile to begin reading.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public long Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private long offset;

        /// <summary> 
        /// This parameter provides the size of blocks to be
        /// read from the specified InFile.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public int BlockSize
        {
            get { return blockSize; }
            set { blockSize = value; }
        }
        private int blockSize;

        /// <summary> 
        /// This parameter provides the Count of Blocks
        /// to read from the specified InFile.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private int count;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a Reads bytes from the InFile
        /// and outputs to the OutFile.
        /// </summary> 

        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(inFile))
            {

                inFile = @"\\.\" + inFile + ":";

            }

            WriteDebug("VolumeName: " + inFile);

            InvokeIR.PowerForensics.Main.dd(inFile, outFile, offset, blockSize, count);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End ExportPowerDDCommand class. 

    #endregion ExportPowerDDCommand

}