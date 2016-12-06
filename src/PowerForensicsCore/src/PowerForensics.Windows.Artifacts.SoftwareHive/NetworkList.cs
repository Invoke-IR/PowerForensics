using System;
using System.Text;
using System.Collections.Generic;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.SoftwareHive
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkList
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime WriteTimeUtc;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly string ProfileGuid;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint Source;

        /// <summary>
        /// 
        /// </summary>
        public readonly string DnsSuffix;

        /// <summary>
        /// 
        /// </summary>
        public readonly string FirstNetwork;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] DefaultGatewayMac;

        #endregion Properties

        #region Constructors

        private NetworkList(NamedKey nk, byte[] bytes)
        {
            WriteTimeUtc = nk.WriteTime;

            foreach (ValueKey vk in nk.GetValues(bytes))
            {
                switch (vk.Name)
                {
                    case "ProfileGuid":
                        ProfileGuid = (string)vk.GetData(bytes);
                        break;
                    case "Description":
                        Description = (string)vk.GetData(bytes);
                        break;
                    case "Source":
                        Source = BitConverter.ToUInt32((byte[])vk.GetData(bytes), 0x00);
                        break;
                    case "DnsSuffix":
                        DnsSuffix = (string)vk.GetData(bytes);
                        break;
                    case "FirstNetwork":
                        FirstNetwork = (string)vk.GetData(bytes);
                        break;
                    case "DefaultGatewayMac":
                        DefaultGatewayMac = (byte[])vk.GetData(bytes);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static NetworkList[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);
            WindowsVersion version = WindowsVersion.Get(volume);
            if (version.CurrentVersion.CompareTo(new Version("6.0")) >= 0)
            {
                return GetInstancesByPath(Helper.GetVolumeLetter(volume) + @"\Windows\system32\config\SOFTWARE");
            }
            else
            {
                throw new Exception("The NetworkList key is only available on Windows Vista an newer Operating Systems.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static NetworkList[] GetInstancesByPath(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "SOFTWARE"))
            {
                string Key = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Signatures";

                byte[] bytes = Registry.RegistryHelper.GetHiveBytes(hivePath);

                NamedKey[] SignatureKey = NamedKey.GetInstances(bytes, hivePath, Key);

                List<NetworkList> nlList = new List<NetworkList>();

                foreach (NamedKey key in SignatureKey)
                {
                    if (key.NumberOfSubKeys != 0)
                    {
                        foreach (NamedKey nk in key.GetSubKeys(bytes))
                        {
                            nlList.Add(new NetworkList(nk, bytes));
                        }
                    }
                }
                return nlList.ToArray();
            }
            else
            {
                throw new Exception("Invalid SOFTWARE hive provided to -HivePath parameter.");
            }
        }

        #endregion Static Methods
    }

}
