using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Management.Automation;
using System.Runtime.InteropServices;
using InvokeIR.PowerForensics;

namespace InvokeIR.PowerForensics.TSK
{

    public class MBR
    {    

        public struct MASTER_BOOT_RECORD
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 440)]
            private byte[] MBRCodeArea;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4)]
            private byte[] DiskSignature;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 2)]
            private byte[] NotUsed;
            public MBR_PARTITION_TABLE_ENTRY partitionEntry1;
            public MBR_PARTITION_TABLE_ENTRY partitionEntry2;
            public MBR_PARTITION_TABLE_ENTRY partitionEntry3;
            public MBR_PARTITION_TABLE_ENTRY partitionEntry4;
            private byte _AA;
            private byte _55;

            public MASTER_BOOT_RECORD(byte[] MBRBytes)
            {
                MBRCodeArea = MBRBytes.Take(440).ToArray();
                DiskSignature = MBRBytes.Skip(440).Take(4).ToArray();
                NotUsed = MBRBytes.Skip(444).Take(2).ToArray();
                partitionEntry1 = new MBR_PARTITION_TABLE_ENTRY(MBRBytes.Skip(446).Take(16).ToArray());
                partitionEntry2 = new MBR_PARTITION_TABLE_ENTRY(MBRBytes.Skip(462).Take(16).ToArray());
                partitionEntry3 = new MBR_PARTITION_TABLE_ENTRY(MBRBytes.Skip(478).Take(16).ToArray());
                partitionEntry4 = new MBR_PARTITION_TABLE_ENTRY(MBRBytes.Skip(494).Take(16).ToArray());
                _AA = MBRBytes[510];
                _55 = MBRBytes[511];
            }
        }

        private const byte BOOTABLE = 0x80;
        private const byte NON_BOOTABLE = 0x00;

        private enum PARTITION_TYPE
        {
            EMPTY = 0x00,
            FAT12 = 0x01,
            FAT16_16MB_32MB = 0x04,
            MS_EXTENDED = 0x05,
            FAT16_32MB_2GB = 0x06,
            NTFS = 0x07,
            FAT32_CHS = 0x0b,
            FAT32_LBA = 0x0c,
            FAT16_32MB_2GB_LBA = 0x0e, 
            MS_EXTENDED_LBA = 0x0f,
            HIDDEN_FAT12_CHS = 0x11, 
            HIDDEN_FAT16_16MB_32MB_CHS = 0x14, 
            HIDDEN_FAT16_32MB_2GB_CHS = 0x16,
            HIDDEN_FAT32_CHS = 0x1b,
            HIDDEN_FAT32_LBA = 0x1c,
            HIDDEN_FAT16_32MB_2GB_LBA = 0x1e, 
            MS_MBR_DYNAMIC_DISK = 0x42,
            SOLARIS_X86 = 0x82,
            LINUX_SWAP = 0x82,
            LINUX = 0x83,
            HIBERNATION = 0x84,
            LINUX_EXTENDED = 0x85,
            NTFS_VOLUME_SET = 0x86,
            NTFS_VOLUME_SET_1 = 0x87,
            HIBERNATION_1 = 0xa0,
            HIBERNATION_2 = 0xa1,
            FREEBSD = 0xa5,
            OPENBSD = 0xa6,
            MACOSX = 0xa8,
            NETBSD = 0xa9,
            MAC_OSX_BOOT = 0xab,
            BSDI = 0xb7,
            BSDI_SWAP = 0xb8,
            EFI_GPT_DISK = 0xee,
            EFI_SYSTEM_PARTITION = 0xef,
            VMWARE_FILE_SYSTEM = 0xfb,
            VMWARE_SWAP = 0xfc
        }

        public struct MBR_PARTITION_TABLE_ENTRY
        {
            public bool Bootable;
            internal byte startingHeadNumber;
            internal byte startingSectorNumber;
            internal byte startingCylinderHigh2;
            internal byte startingCylinderLow8;
            public string SystemID;
            internal byte endingHeadNumber;
            internal byte endingSectorNumber;
            internal byte endingCylinderHigh2;
            internal byte endingCylinderHigh8;
            public uint RelativeSector;
            public uint TotalSectors;
        
            internal MBR_PARTITION_TABLE_ENTRY(byte[] bytes)
            {
                Bootable = (bytes[0] == BOOTABLE);
                startingHeadNumber = bytes[1];
                startingSectorNumber = bytes[2];// &0xFC;
                startingCylinderHigh2 = bytes[2];// &0x03;
                startingCylinderLow8 = bytes[3];
                SystemID = Enum.GetName(typeof(PARTITION_TYPE), bytes[4]);
                endingHeadNumber = bytes[5];
                endingSectorNumber = bytes[6];// &0xFC;
                endingCylinderHigh2 = bytes[6];// &0x03;
                endingCylinderHigh8 = bytes[7];
                RelativeSector = BitConverter.ToUInt32(bytes, 8);
                TotalSectors = BitConverter.ToUInt32(bytes, 12);
            }

        }

        public static MASTER_BOOT_RECORD Get(FileStream streamToRead, string DrivePath)
        {
            byte[] MBRBytes = Win32.readDrive(streamToRead, 0, 512);
            return new MASTER_BOOT_RECORD(MBRBytes);
        }

        #region GetMMlsCommand
        /// <summary> 
        /// This class implements the Get-MMls cmdlet. 
        /// </summary> 

        [Cmdlet(VerbsCommon.Get, "MMls", SupportsShouldProcess = true)]
        public class GetMMlsCommand : PSCmdlet
        {
            #region Parameters

            /// <summary> 
            /// This parameter provides the DriveName for the 
            /// raw bytes that will be returned.
            /// </summary> 

            [Parameter(Mandatory = true)]
            public string DrivePath
            {
                get { return drivePath; }
                set { drivePath = value; }
            }
            private string drivePath;

            #endregion Parameters

            #region Cmdlet Overrides

            /// <summary> 
            /// The ProcessRecord outputs the raw bytes of the specified File
            /// </summary> 

            protected override void ProcessRecord()
            {

                IntPtr hDrive = Win32.getHandle(DrivePath);
                FileStream streamToRead = Win32.getFileStream(hDrive);

                MASTER_BOOT_RECORD PartitionTable = InvokeIR.PowerForensics.TSK.MBR.Get(streamToRead, drivePath);
                WriteObject(PartitionTable.partitionEntry1);
                WriteObject(PartitionTable.partitionEntry2);
                WriteObject(PartitionTable.partitionEntry3);
                WriteObject(PartitionTable.partitionEntry4);
                streamToRead.Close();

            } // ProcessRecord 

            #endregion Cmdlet Overrides

        } // End GetFSstatCommand class. 

        #endregion GetMMlsCommand
    
    }

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

            IntPtr hVolume = Win32.getHandle(volumeName);

            WriteObject(NTFS.NTFSVolumeData.Get(hVolume));

            Win32.CloseHandle(hVolume);


            //NTFSVolumeData volumeData = InvokeIR.PowerForensics.Main.getVolumeDataInformation(volumeName);
            //WriteObject(volumeData);

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
          
            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";

            }

            WriteDebug("VolumeName: " + volume);

            IntPtr hVolume = InvokeIR.PowerForensics.Win32.getHandle(volume);
            FileStream streamToRead = InvokeIR.PowerForensics.Win32.getFileStream(hVolume);

            if (this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {

                indexNumber = InvokeIR.PowerForensics.NTFS.IndexNumber.Get(hVolume, streamToRead, filePath);

            }

            WriteObject(InvokeIR.PowerForensics.NTFS.FileRecord.Get(hVolume, streamToRead, indexNumber));
            streamToRead.Close();

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetIStatCommand

    #region GetICatCommand
    /// <summary> 
    /// This class implements the Get-ICat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "ICat", SupportsShouldProcess = true)]
    public class GetICatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// raw bytes that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            IntPtr hVolume = InvokeIR.PowerForensics.Win32.getHandle(@"\\.\C:");

            FileStream streamToRead = InvokeIR.PowerForensics.Win32.getFileStream(hVolume);

            WriteObject(InvokeIR.PowerForensics.NTFS.FileRecord.getFile(hVolume, streamToRead, filePath));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetIStatCommand

}
