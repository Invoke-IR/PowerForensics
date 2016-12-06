namespace PowerForensics.FileSystems.HFSPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class AllocationFile
    {
        #region Static Methods

        /// <summary>
        /// Returns the Contents of the HFS+ Allocation File.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static byte[] GetContent(string volumeName)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        public static bool IsAllocationBlockUsed(string volumeName, uint blockNumber)
        {
            // Get VolumeHeader
            VolumeHeader volHeader = VolumeHeader.Get(volumeName);

            // Detemine which byte to look at
            uint bytePosition = blockNumber / 0x8;

            // Determine which of AllocationFile's blocks the byte belongs to
            uint fileBlock = bytePosition / volHeader.BlockSize;

            uint relativeBlock = 0;
            byte[] blockBytes = null;

            foreach (ExtentDescriptor extent in volHeader.AllocationFile.Extents)
            {
                if (fileBlock < relativeBlock + extent.BlockCount)
                {
                    uint blockToRead = fileBlock - relativeBlock + extent.StartBlock;
                    blockBytes = Helper.readDrive(volumeName, (blockToRead * volHeader.BlockSize), volHeader.BlockSize);
                    break;
                }
                else
                {
                    relativeBlock += extent.BlockCount;
                }
            }

            // Pick the right byte from the Sector
            byte byteToCheck = blockBytes[(blockNumber / 8) % volHeader.BlockSize];
            byte position = (byte)(blockNumber % 8);

            // Need to come back and ensure this is correct (it appears to be opposite of what i'd expect)
            return (byteToCheck & (1 >> (position))) != 0;
        }

        #endregion Static Methods
    }
}
