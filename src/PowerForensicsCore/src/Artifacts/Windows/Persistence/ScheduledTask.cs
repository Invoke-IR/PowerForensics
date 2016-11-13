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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ScheduledTask Get(string path)
        {
            return Get(FileRecord.Get(path, true).GetContent());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static ScheduledTask Get(byte[] bytes)
        {
            return new ScheduledTask(Encoding.Unicode.GetString(bytes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static ScheduledTask[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            return null;
        }

        #endregion StaticMethods
    }

    #endregion ScheduledTask
}
