using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS
{
    #region AttrDefClass

    class AttrDef
    {

        #region Enums

        [FlagsAttribute]
        internal enum ATTR_DEF_ENTRY
        {
            INDEX = 0x02,
            ALWAYS_RESIDENT = 0x40,
            ALWAYS_NONRESIDENT = 0x80
        }
        
        #endregion Enums

        #region Structs

        internal struct ATTR_DEF
        {
            internal string Name;
            internal uint TypeIdentified;
            internal uint DisplayRule;
            internal uint CollationRule;
            internal uint Flags;
            internal ulong MinSize;
            internal ulong MaxSize;

            internal ATTR_DEF(byte[] bytes)
            {
                Name = Encoding.Unicode.GetString(bytes.Take(0x80).ToArray()).TrimEnd('\0');
                TypeIdentified = BitConverter.ToUInt32(bytes, 0x80);
                DisplayRule = BitConverter.ToUInt32(bytes, 0x84);
                CollationRule = BitConverter.ToUInt32(bytes, 0x88);
                Flags = BitConverter.ToUInt32(bytes, 0x8C);
                MinSize = BitConverter.ToUInt64(bytes, 0x90);
                MaxSize = BitConverter.ToUInt64(bytes, 0x98);
            }

        }

        #endregion Structs

        #region Properties

        public readonly string Name;
        public readonly uint Type;
        public readonly string Flags;
        public readonly ulong MinSize;
        public readonly ulong MaxSize;

        #endregion Properties

        #region Constructors

        internal AttrDef(byte[] bytes)
        {
            ATTR_DEF attrDefStruct = new ATTR_DEF(bytes);

            Name = attrDefStruct.Name;
            Type = attrDefStruct.TypeIdentified;
            Flags = ((ATTR_DEF_ENTRY)attrDefStruct.Flags).ToString();
            MinSize = attrDefStruct.MinSize;
            MaxSize = attrDefStruct.MaxSize;
        }

        #endregion Constructors

        #region GetInstancesMethod

        internal static AttrDef[] GetInstances(string volumeName)
        {
            // Get correct volume name from user input
            NativeMethods.getVolumeName(ref volumeName);

            // Get handle to Logical Volume
            IntPtr hVolume = NativeMethods.getHandle(volumeName);

            // Instantiate a List of AttrDef objects for output
            List<AttrDef> adList = new List<AttrDef>();

            // Create a FileStream object for the Volume
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                // Instantiate a VolumeData object
                VolumeData volData = new VolumeData(hVolume);

                ulong attrDefOffset = ((volData.MFTStartCluster * (ulong)volData.BytesPerCluster) + ((ulong)volData.BytesPerMFTRecord * 4));

                // Get the MFTRecord for the file with a record index of 4 ($AttrDef)
                MFTRecord record = new MFTRecord(NativeMethods.readDrive(streamToRead, attrDefOffset, (ulong)volData.BytesPerMFTRecord));

                // Get the content of the $AttrDef file in a byte array
                byte[] bytes = MFTRecord.getFile(streamToRead, record);

                // Iterate through 160 byte chunks (representing an AttrDef object)
                for (int i = 0; (i < bytes.Length) && (bytes[i] != 0); i += 160)
                {
                    byte[] attrDefBytes = new byte[160];

                    Array.Copy(bytes, i, attrDefBytes, 0, attrDefBytes.Length);
                    
                    // Intantiate a new AttrDef object and add it to the adList List of AttrDef objects
                    adList.Add(new AttrDef(attrDefBytes));
                }
            }

            NativeMethods.CloseHandle(hVolume);

            // Return an array of AttrDef objects
            return adList.ToArray();
        }

        #endregion GetInstancesMethod

    }

    #endregion AttrDefClass
}
