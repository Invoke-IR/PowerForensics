using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.UserHive
{
    /// <summary>
    /// 
    /// </summary>
    public class WordWheelQuery
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string User;

        /// <summary>
        /// 
        /// </summary>
        public readonly string SearchString;

        #endregion Properties

        #region Constructors

        private WordWheelQuery(string user, string path)
        {
            User = user;
            SearchString = path;
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static WordWheelQuery[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string Key = @"Software\Microsoft\Windows\CurrentVersion\Explorer\WordWheelQuery";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey nk = null;

                try
                {
                    nk = NamedKey.Get(bytes, hivePath, Key);
                }
                catch
                {
                    return null;
                }

                ValueKey MRUList = ValueKey.Get(bytes, hivePath, Key, "MRUListEx");

                WordWheelQuery[] dataStrings = new WordWheelQuery[nk.NumberOfValues - 1];

                byte[] MRUListBytes = (byte[])MRUList.GetData(bytes);

                for (int i = 0; i < MRUListBytes.Length - 4; i += 4)
                {
                    uint MRUValue = BitConverter.ToUInt32(MRUListBytes, i);
                    string SearchString = null;
                    try
                    {
                        SearchString = (string)ValueKey.Get(bytes, hivePath, Key, MRUValue.ToString()).GetData(bytes);
                    }
                    catch
                    {
                        SearchString = Encoding.Unicode.GetString((byte[])ValueKey.Get(bytes, hivePath, Key, MRUValue.ToString()).GetData(bytes));
                    }
                    dataStrings[i / 4] = new WordWheelQuery(RegistryHelper.GetUserHiveOwner(hivePath), SearchString);
                }

                return dataStrings;
            }
            else
            {
                throw new Exception("Invalid NTUSER.DAT hive provided to -HivePath parameter.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static WordWheelQuery[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<WordWheelQuery> list = new List<WordWheelQuery>();

            foreach (string hivePath in RegistryHelper.GetUserHiveInstances(volume))
            {
                try
                {
                    list.AddRange(Get(hivePath));
                }
                catch
                {

                }
            }

            return list.ToArray();
        }

        #endregion Static Methods
    }
}
