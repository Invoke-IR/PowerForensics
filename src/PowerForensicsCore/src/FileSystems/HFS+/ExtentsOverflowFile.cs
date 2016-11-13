using System;
using System.Collections.Generic;
using PowerForensics.HFSPlus.BTree;

namespace PowerForensics.HFSPlus
{
    public class ExtentsOverflowFile
    {
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
    }

    public class ExtentsOverflowRecord : Record
    {
        #region Enums

        public enum FORK_TYPE
        {
            Data = 0x00,
            Resource = 0xFF
        }

        #endregion Enums

        #region Properties

        private readonly string VolumeName;
        private readonly string FileName;
        private readonly ushort KeyLength;

        public readonly FORK_TYPE ForkType;
        public readonly uint CatalogNodeId;
        public readonly uint RelativeStartBlock;

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

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static ExtentsOverflowRecord Get(byte[] bytes, int offset, string volumeName, string fileName)
        {
            return new ExtentsOverflowRecord(bytes, offset, volumeName, fileName);
        }

        #endregion StaticMethods
    }
}