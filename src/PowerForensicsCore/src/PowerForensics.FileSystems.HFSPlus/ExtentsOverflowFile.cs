using System;
using PowerForensics.FileSystems.HFSPlus.BTree;

namespace PowerForensics.FileSystems.HFSPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtentsOverflowFile
    {
        #region Static Methods

        /// <summary>
        /// Returns the contents of the HFS+ Extents Overflow File.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static byte[] GetContent(string volumeName)
        {
            return VolumeHeader.Get(volumeName).GetExtentsOverflowFileBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static Node GetHeaderNode(string volumeName)
        {
            return Node.GetHeaderNode(volumeName, "ExtentsOverflow");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        public static Node GetNode(string volumeName, uint nodeNumber)
        {
            return Node.Get(volumeName, "ExtentsOverflow", nodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        public static byte[] GetNodeBytes(string volumeName, uint nodeNumber)
        {
            return Node.GetBytes(volumeName, "ExtentsOverflow", nodeNumber);
        }

        #endregion Static Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExtentsOverflowRecord : Record
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        public enum FORK_TYPE
        {
            /// <summary>
            /// 
            /// </summary>
            Data = 0x00,
            
            /// <summary>
            /// 
            /// </summary>
            Resource = 0xFF
        }

        #endregion Enums

        #region Properties

        private readonly string VolumeName;

        private readonly string FileName;

        private readonly ushort KeyLength;

        /// <summary>
        /// 
        /// </summary>
        public readonly FORK_TYPE ForkType;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint CatalogNodeId;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RelativeStartBlock;


        /// <summary>
        /// 
        /// </summary>
        public readonly ExtentDescriptor[] Extents;

        #endregion Properties

        #region Constructor

        private ExtentsOverflowRecord(byte[] bytes, int offset, string volumeName, string fileName)
        {
            VolumeHeader volHeader = VolumeHeader.Get(volumeName);
            VolumeName = volumeName;
            FileName = fileName;
            KeyLength = Helper.SwapEndianness(BitConverter.ToUInt16(bytes, offset + 0x00));
            ForkType = (FORK_TYPE)bytes[offset + 0x02];
            CatalogNodeId = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x04));
            RelativeStartBlock = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x08));
            Extents = ExtentDescriptor.GetInstances(bytes, offset + 0x0C, volumeName, volHeader.BlockSize);
        }

        #endregion Constructor

        #region Static Methods

        internal static ExtentsOverflowRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new ExtentsOverflowRecord(bytes, offset, volumeName, fileName);
        }

        #endregion Static Methods
    }
}