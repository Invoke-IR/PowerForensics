using System;
using System.Collections.Generic;

namespace PowerForensics.HFSPlus
{
    public class ForkData
    {
        #region Properties

        private readonly string VolumeName;
        public readonly ulong LogicalSize;
        public readonly uint ClumpSize;
        public readonly uint TotalBlocks;
        public readonly ExtentDescriptor[] Extents;

        #endregion Properties

        #region Constructors

        private ForkData(byte[] bytes, int offset, string volumeName, uint blockSize)
        {
            VolumeName = volumeName;
            LogicalSize = Helper.SwapEndianness(BitConverter.ToUInt64(bytes, offset));
            ClumpSize = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x08));
            TotalBlocks = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x0C));
            Extents = ExtentDescriptor.GetInstances(bytes, offset + 0x10, volumeName, blockSize);
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        internal static ForkData Get(byte[] bytes, int offset, string volumeName, uint blockSize)
        {
            return new ForkData(bytes, offset, volumeName, blockSize);
        }

        #endregion StaticMethods

        #region InstanceMethods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetContent()
        {
            List<byte> byteList = new List<byte>();
            uint extentBlocks = 0;

            foreach (ExtentDescriptor extent in Extents)
            {
                extentBlocks += extent.BlockCount;
                byteList.AddRange(extent.GetContent());
            }

            if (TotalBlocks == extentBlocks)
            {
                return Helper.GetSubArray(byteList.ToArray(), 0, (long)LogicalSize);
            }
            else
            {
                // Need to get some stuff from Extent Overflow File
                Console.WriteLine("Portion of extents exist in Extent Overflow file");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetSlack()
        {
            List<byte> byteList = new List<byte>();
            uint extentBlocks = 0;

            foreach (ExtentDescriptor extent in Extents)
            {
                extentBlocks += extent.BlockCount;
                byteList.AddRange(extent.GetContent());
            }

            if (TotalBlocks == extentBlocks)
            {
                ExtentDescriptor extent = Extents[Extents.Length - 1];
                byte[] bytes = extent.GetContent();
                return null; //Helper.GetSubArray(bytes, , );
            }
            else
            {
                // Need to get some stuff from Extent Overflow File
                Console.WriteLine("Portion of extents exist in Extent Overflow file");
                return null;
            }
        }

        #endregion InstanceMethods
    }

    public class ExtentDescriptor
    {
        #region Properties

        private readonly string VolumeName;
        private readonly uint BlockSize;
        public readonly uint StartBlock;
        public readonly uint BlockCount;

        #endregion Properties

        #region Constructors

        private ExtentDescriptor(byte[] bytes, int offset, string volumeName, uint blockSize)
        {
            VolumeName = volumeName;
            BlockSize = blockSize;
            StartBlock = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset));
            BlockCount = Helper.SwapEndianness(BitConverter.ToUInt32(bytes, offset + 0x04));
        }

        #endregion Constructors

        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        internal static ExtentDescriptor[] GetInstances(byte[] bytes, int offset, string volumeName, uint blockSize)
        {
            List<ExtentDescriptor> extentList = new List<ExtentDescriptor>();

            for (int i = 0; i < 0x08; i++)
            {
                ExtentDescriptor extent = Get(bytes, offset + (i * 0x08), volumeName, blockSize);

                if (extent.BlockCount != 0)
                {
                    extentList.Add(extent);
                }
            }

            return extentList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="volumeName"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        private static ExtentDescriptor Get(byte[] bytes, int offset, string volumeName, uint blockSize)
        {
            return new ExtentDescriptor(bytes, offset, volumeName, blockSize);
        }

        #endregion StaticMethods

        #region InstanceMethods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] GetContent()
        {
            VolumeHeader header = VolumeHeader.Get(VolumeName);

            return Helper.readDrive(VolumeName, (long)StartBlock * BlockSize, BlockCount * BlockSize);
        }

        #endregion InstanceMethods
    }
}
