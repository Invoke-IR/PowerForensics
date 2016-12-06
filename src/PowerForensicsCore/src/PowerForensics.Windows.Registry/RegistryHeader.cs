using System;
using System.Text;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class RegistryHeader
    {
        #region Constants

        internal const int HBINOFFSET = 0x1000;

        #endregion Constants

        #region Enums

        enum FILE_TYPE
        {
            Normal = 0x00,
            TransactionLog = 0x01
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string Signature;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint PrimarySequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint SecondarySequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime ModificationTime;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly Version Version;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FileType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RootKeyOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint HiveBinsDataSize;

        /// <summary>
        /// 
        /// </summary>
        public readonly string HivePath;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Checksum;

        #endregion Properties

        #region Constructors

        internal RegistryHeader(byte[] bytes)
        {
            Signature = Encoding.ASCII.GetString(bytes, 0x00, 0x04);

            if(Signature != "regf")
            {
                throw new Exception("Invalid Registry Header");
            }

            PrimarySequenceNumber = BitConverter.ToUInt32(bytes, 0x04);
            SecondarySequenceNumber = BitConverter.ToUInt32(bytes, 0x08);
            ModificationTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64(bytes, 0x0C));
            Version = new Version(BitConverter.ToInt32(bytes, 0x14), BitConverter.ToInt32(bytes, 0x18));
            FileType = Enum.GetName(typeof(FILE_TYPE), 0x1C);
            RootKeyOffset = BitConverter.ToUInt32(bytes, 0x24);
            HiveBinsDataSize = BitConverter.ToUInt32(bytes, 0x28);
            HivePath = Encoding.Unicode.GetString(bytes, 0x30, 0x40).Split('\0')[0];
            Checksum = BitConverter.ToUInt32(bytes, 0x1FC);
        }

        #endregion Constructors

        #region Static Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string path)
        {
            FileRecord record = FileRecord.Get(path, true); 
            byte[] bytes = record.GetContent();

            // Registry Header
            return Helper.GetSubArray(bytes, 0x00, 0x200);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static RegistryHeader Get(string path)
        {
            return new RegistryHeader(RegistryHeader.GetBytes(path));
        }

        #endregion Static Methods
    }
}
