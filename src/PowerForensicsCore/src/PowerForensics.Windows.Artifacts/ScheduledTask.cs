using System.Text;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.Artifacts
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduledTask
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Path;

        /// <summary>
        /// 
        /// </summary>
        public string Name;

        /// <summary>
        /// 
        /// </summary>
        public string Author;

        /// <summary>
        /// 
        /// </summary>
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
}
