using System;

namespace PowerForensics.Ntfs
{
    #region BadClusClass

    public class BadClus
    {
        #region Properties

        public readonly long Cluster;
        public readonly bool Bad;

        #endregion Properties

        #region Constructors

        internal BadClus(long cluster, bool bad)
        {
            Cluster = cluster;
            Bad = bad;
        }

        #endregion Constructors

        #region StaticMethods
        
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

        #endregion StaticMethods
    }

    #endregion BadClusClass
}
