using PowerForensics.Ntfs;
using System.Collections.Generic;

namespace PowerForensics.Artifacts
{
    #region AlternateDataStreamClass

    public class AlternateDataStream
    {
        #region Properties

        public readonly string FullName;
        public readonly string Name;
        public readonly string StreamName;

        #endregion Properties

        #region Constructors

        private AlternateDataStream(string fullName, string name, string streamName)
        {
            FullName = fullName;
            Name = name;
            StreamName = streamName;
        }

        #endregion Constructors

        #region StaticMethods

        public static AlternateDataStream[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<AlternateDataStream> adsList = new List<AlternateDataStream>();

            FileRecord[] records = FileRecord.GetInstances(volume);

            foreach (FileRecord record in records)
            {
                adsList.AddRange(GetInstances(record));
            }

            return adsList.ToArray();
        }

        public static AlternateDataStream[] GetInstancesByPath(string path)
        {
            FileRecord record = FileRecord.Get(path, false);
            return GetInstances(record);
        }

        private static AlternateDataStream[] GetInstances(FileRecord record)
        {
            List<AlternateDataStream> adsList = new List<AlternateDataStream>();

            if (record.Attribute != null)
            {
                foreach (FileRecordAttribute attr in record.Attribute)
                {
                    if (attr.Name == FileRecordAttribute.ATTR_TYPE.DATA)
                    {
                        if (attr.NameString.Length > 0)
                        {
                            adsList.Add(new AlternateDataStream(record.FullName, record.Name, attr.NameString));
                        }
                    }
                }
            }

            return adsList.ToArray();
        }

        #endregion StaticMethods
    }

    #endregion AlternateDataStreamClass
}
