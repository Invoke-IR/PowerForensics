using System;
using System.Collections.Generic;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts
{
    /// <summary>
    /// 
    /// </summary>
    public class RunKey
    {
        #region Properties
       
        /// <summary>
        /// 
        /// </summary>
        public readonly string AutoRunLocation;

        /// <summary>
        /// 
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 
        /// </summary>
        public readonly string ImagePath;

        #endregion Properties
        
        #region Constructors
        
        private RunKey(string location, ValueKey vk)
        {
            AutoRunLocation = location;
            Name = vk.Name;
            ImagePath = (string)vk.GetData();
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static RunKey[] GetInstances(string volume)
        {
            Helper.getVolumeName(ref volume);

            List<RunKey> list = new List<RunKey>();

            try
            {
                list.AddRange(Get(Helper.GetVolumeLetter(volume) + @"\Windows\system32\config\SOFTWARE"));
            }
            catch
            {

            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static RunKey[] Get(string hivePath)
        {
            List<string> Keys = new List<string>();
            string AutoRunLocation = null;

            if (RegistryHelper.isCorrectHive(hivePath, "SOFTWARE"))
            {
                Keys.AddRange(new string[] { @"Microsoft\Windows\CurrentVersion\Run", @"Microsoft\Windows\CurrentVersion\RunOnce", @"Wow6432Node\Microsoft\Windows\CurrentVersion\Run" });
                AutoRunLocation = @"HKLM\SOFTWARE\";
            }
            else if (RegistryHelper.isCorrectHive(hivePath, "NTUSER.DAT"))
            {
                Keys.AddRange(new string[] { @"Software\Microsoft\Windows\CurrentVersion\Run", @"Software\Microsoft\Windows\CurrentVersion\RunOnce" });
                AutoRunLocation = @"USER\" + RegistryHelper.GetUserHiveOwner(hivePath) + "\\";

            }
            else
            {
                throw new Exception("Invalid SOFTWARE or NTUSER.DAT hive provided.");
            }

            byte[] bytes = RegistryHelper.GetHiveBytes(hivePath);
            List<RunKey> runList = new List<RunKey>();

            foreach (string key in Keys)
            {
                try
                {
                    NamedKey run = NamedKey.Get(bytes, hivePath, key);
                    if (run.NumberOfValues > 0)
                    {
                        foreach (ValueKey vk in run.GetValues(bytes))
                        {
                            runList.Add(new RunKey(AutoRunLocation + key, vk));
                        }
                    }
                }
                catch
                {

                }
            }

            return runList.ToArray();
        }

        #endregion Static Methods
    }
}
