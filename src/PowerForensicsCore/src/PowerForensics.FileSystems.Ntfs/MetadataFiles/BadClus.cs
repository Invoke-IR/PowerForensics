using System;

namespace PowerForensics.FileSystems.Ntfs
{
    /// <summary>
    /// 
    /// </summary>
    public class BadClus
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly long Cluster;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool Bad;

        #endregion Properties

        #region Constructors

        internal BadClus(long cluster, bool bad)
        {
            Cluster = cluster;
            Bad = bad;
        }

        #endregion Constructors

        #region Static Methods
        
        internal static FileRecord GetFileRecord(string volume)
        {
            Helper.getVolumeName(ref volume);
            return FileRecord.Get(volume, MftIndex.BADCLUS_INDEX, true);
        }

        internal static NonResident GetBadStream(FileRecord fileRecord)
        {
            foreach (FileRecordAttribute attr in fileRecord.Attribute)
            {
                if (attr.NameString == "$Bad")
                {
                    return attr as NonResident;
                }
            }
            throw new Exception("No $Bad attribute found.");
        }

        #endregion Static Methods
    }
}