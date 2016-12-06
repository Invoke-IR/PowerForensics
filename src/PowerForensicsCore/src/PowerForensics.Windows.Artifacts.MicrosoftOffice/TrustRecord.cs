using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.MicrosoftOffice
{
    /// <summary>
    /// 
    /// </summary>
    public class TrustRecord
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly string User;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime TrustTime;

        #endregion Properties

        #region Constructors

        private TrustRecord(byte[] bytes, string user, ValueKey vk)
        {
            User = user;
            Path = vk.Name;
            TrustTime = DateTime.FromFileTimeUtc(BitConverter.ToInt64((byte[])vk.GetData(bytes), 0x00));
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static TrustRecord[] Get(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                string user = RegistryHelper.GetUserHiveOwner(hivePath);
                List<TrustRecord> trList = new List<TrustRecord>();

                byte[] bytes = RegistryHelper.GetHiveBytes(hivePath);
                string OfficeVersion = RegistryHelper.GetOfficeVersion(bytes, hivePath);
                string[] applications = new string[] { "Word", "Excel", "PowerPoint"};

                for(int i = 0; i < applications.Length; i++)
                {
                    try
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"Software\Microsoft\Office\").Append(OfficeVersion).Append("\\").Append(applications[i]).Append(@"\Security\Trusted Documents\TrustRecords");
                        NamedKey nk = NamedKey.Get(bytes, hivePath, sb.ToString());

                        foreach(ValueKey vk in nk.GetValues(bytes))
                        {
                            trList.Add(new TrustRecord(bytes, user, vk));
                        }
                    }
                    catch
                    {

                    }
                }

                return trList.ToArray();
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
        public static TrustRecord[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<TrustRecord> list = new List<TrustRecord>();

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