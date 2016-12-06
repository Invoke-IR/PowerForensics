using PowerForensics.FileSystems.HFSPlus.BTree;

namespace PowerForensics.FileSystems.HFSPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class AttributesFile
    {
        #region Static Methods

        /// <summary>
        /// Returns the content of the HFS+ Attributes File.
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static byte[] GetContent(string volumeName)
        {
            return VolumeHeader.Get(volumeName).GetAttributesFileBytes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        public static Node GetHeaderNode(string volumeName)
        {
            return Node.GetHeaderNode(volumeName, "Attributes");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        public static Node GetNode(string volumeName, uint nodeNumber)
        {
            return Node.Get(volumeName, "Attributes", nodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volumeName"></param>
        /// <param name="nodeNumber"></param>
        /// <returns></returns>
        public static byte[] GetNodeBytes(string volumeName, uint nodeNumber)
        {
            return Node.GetBytes(volumeName, "Attributes", nodeNumber);
        }

        #endregion Static Methods
    }
}