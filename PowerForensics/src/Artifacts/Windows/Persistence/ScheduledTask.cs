using System.Text;
using PowerForensics.Ntfs;

namespace PowerForensics.Artifacts
{
    #region ScheduledTask

    public class ScheduledTask
    {
        #region Properties

        public string Path;
        public string Name;
        public string Author;
        public string Description;

        #endregion Properties

        #region Constructors

        private ScheduledTask(string xml)
        {

        }

        #endregion Constructors

        #region StaticMethods

        public static ScheduledTask Get(string path)
        {
            return Get(FileRecord.Get(path, true).GetContent());
        }

        private static ScheduledTask Get(byte[] bytes)
        {
            return new ScheduledTask(Encoding.Unicode.GetString(bytes));
        }

        public static ScheduledTask[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            return null;
        }

        #endregion StaticMethods
    }

    #endregion ScheduledTask
}
