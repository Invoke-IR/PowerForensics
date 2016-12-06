using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics;
using PowerForensics.Windows.Artifacts.SoftwareHive;
using PowerForensics.FileSystems.Ntfs;

namespace PowerForensics.Windows.Artifacts.ApplicationCompatibilityCache
{
    /// <summary>
    /// 
    /// </summary>
    public class RecentFileCache
    {
        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static string[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            WindowsVersion version = WindowsVersion.Get(volume);
            if (version.CurrentVersion.CompareTo(new Version("6.1")) == 0)
            {
                return GetInstancesByPath(Helper.GetVolumeLetter(volume) + @"\Windows\AppCompat\Programs\RecentFileCache.bcf");
            }
            else
            {
                throw new Exception("The RecentFileCache.bcf file is only available on Windows 7 Operating Systems.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetInstancesByPath(string path)
        {
            byte[] bytes = FileRecord.GetContentBytes(path);

            if (BitConverter.ToUInt32(bytes, 0x00) == 0xFFEEFFFE)
            {
                List<string> dataList = new List<string>();

                int offset = 0x14;

                while (offset < bytes.Length)
                {
                    int length = BitConverter.ToInt32(bytes, offset);
                    dataList.Add(Encoding.Unicode.GetString(bytes, offset + 0x04, length * 2));
                    offset += (length * 2) + 0x06;
                }

                return dataList.ToArray();
            }
            else
            {
                throw new Exception("The RecentFileCache.bcf file is invalid.");
            }
        }

        #endregion Static Methods
    }
}
