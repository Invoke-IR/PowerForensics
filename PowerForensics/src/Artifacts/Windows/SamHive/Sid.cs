using System;
using System.Security.Principal;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class Sid
    {
        #region StaticMethods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static string Get(string volume)
        {
            Helper.getVolumeName(ref volume);
            return GetByPath(Helper.GetVolumeLetter(volume) + @"\Windows\system32\config\SAM");   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hivePath"></param>
        /// <returns></returns>
        public static string GetByPath(string hivePath)
        {
            if (RegistryHelper.isCorrectHive(hivePath, "SAM"))
            {
                ValueKey vk = ValueKey.Get(hivePath, @"SAM\Domains\Account", "V");
                byte[] bytes = (byte[])vk.GetData();
                return Helper.GetSecurityIdentifier(Helper.GetSubArray(bytes, bytes.Length - 0x18, 0x18));
            }
            else
            {
                throw new Exception("Invalid SAM hive provided to -HivePath parameter.");
            }
        }
        
        #endregion StaticMethods
    }
}
