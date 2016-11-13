using System;
using System.Collections.Generic;
using PowerForensics.Ntfs;

namespace PowerForensics.Registry
{
    #region HelperClass

    public class RegistryHelper
    {
        #region StaticMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetHiveBytes(string path)
        {
            FileRecord record = FileRecord.Get(path, true);
            return record.GetContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static NamedKey GetRootKey(string path)
        {
            byte[] bytes = GetHiveBytes(path);

            RegistryHeader header = new RegistryHeader(Helper.GetSubArray(bytes, 0x00, 0x200));
            int offset = (int)header.RootKeyOffset + RegistryHeader.HBINOFFSET;
            int size = Math.Abs(BitConverter.ToInt32(bytes, offset));

            return new NamedKey(Helper.GetSubArray(bytes, offset, size), path, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static NamedKey GetRootKey(byte[] bytes, string path)
        {
            #region RegistryHeader

            RegistryHeader header = new RegistryHeader(Helper.GetSubArray(bytes, 0x00, 0x200));

            #endregion RegistryHeader

            int offset = (int)header.RootKeyOffset + RegistryHeader.HBINOFFSET;
            int size = Math.Abs(BitConverter.ToInt32(bytes, offset));

            return new NamedKey(Helper.GetSubArray(bytes, offset, size), path, "");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <param name="hivetype"></param>
        /// <returns></returns>
        internal static bool isCorrectHive(string hivePath, string hivetype)
        {
            if (RegistryHeader.Get(hivePath).HivePath.ToUpper().Contains(hivetype))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        internal static string GetUserHiveOwner(string hivePath)
        {
            return hivePath.Split('\\')[2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        internal static string[] GetUserHiveInstances(string volume)
        {
            List<string> userHiveList = new List<string>();

            IndexEntry[] entries = null;

            try
            {
                entries = IndexEntry.GetInstances(Helper.GetVolumeLetter(volume) + @"\Users");
            }
            catch
            {
                try
                {
                    entries = IndexEntry.GetInstances(Helper.GetVolumeLetter(volume) + @"\Documents and Settings");
                }
                catch
                {
                    throw new Exception("Could not locate User Registry Hives.");
                }
            }

            foreach (IndexEntry e in entries)
            {
                try
                {
                    userHiveList.Add(IndexEntry.Get(e.FullName + @"\NTUSER.DAT").FullName);
                }
                catch
                {

                }
            }

            return userHiveList.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        internal static string GetOfficeVersion(byte[] bytes, string hivePath)
        {
            NamedKey OfficeKey = null;

            try
            {
                OfficeKey = NamedKey.Get(bytes, hivePath, @"Software\Microsoft\Office");
            }
            catch
            {
                throw new Exception(String.Format("Microsoft Office is not installed on this system"));
            }

            foreach (NamedKey nk in OfficeKey.GetSubKeys(bytes))
            {
                if (nk.Name.Contains(@".0"))
                {
                    if (nk.Name != "8.0")
                    {
                        return nk.FullName.Split('\\')[4];
                    }
                }
            }

            throw new Exception("Could not locate the Microsoft Office registry key");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static NamedKey GetOfficeKey(byte[] bytes, string path)
        {
            string key = @"Software\Microsoft\Office";

            NamedKey OfficeKey = null;

            try
            {
                OfficeKey = NamedKey.Get(bytes, path, key);
            }
            catch
            {
                throw new Exception(String.Format("Microsoft Office is not installed on this system"));
            }

            foreach (NamedKey nk in OfficeKey.GetSubKeys(bytes))
            {
                if (nk.Name.Contains(@".0"))
                {
                    if (nk.Name != "8.0")
                    {
                        return nk;
                    }
                }
            }

            throw new Exception("Could not locate the Microsoft Office registry key");
        }

        #endregion StaticMethods
    }

    #endregion HelperClass
}
