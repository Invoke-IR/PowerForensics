using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.PowerForensics.NTFS.MFT;

namespace InvokeIR.PowerForensics.Artifacts
{
    public class Prefetch
    {

        const string Magic = "SCCA";

        enum PREFETCH_VERSION
        {
            WINDOWS_8 = 0x1A,
            WINDOWS_7 = 0x17,
            WINDOWS_XP = 0x11
        }

        #region PrefetchParameters

        private string version;
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }

        /*private string md5;
        public string MD5
        {
            get
            {
                return md5;
            }
            set
            {
                md5 = value;
            }
        }*/

        private string pathhash;
        public string PathHash
        {
            get
            {
                return pathhash;
            }
            set
            {
                pathhash = value;
            }
        }

        private int dependencycount;
        public int DependencyCount
        {
            get
            {
                return dependencycount;
            }
            set
            {
                dependencycount = value;
            }
        }

        private DateTime[] prefetchaccesstime;
        public DateTime[] PrefetchAccessTime
        {
            get
            {
                return prefetchaccesstime;
            }
            set
            {
                prefetchaccesstime = value;
            }
        }

        /*private DateTime prefetchborntime;
        public DateTime PrefetchBornTime
        {
            get
            {
                return prefetchborntime;
            }
            set
            {
                prefetchborntime = value;
            }
        }
        */

        /*private string programborntime;
        public string ProgramBornTime
        {
            get
            {
                return programborntime;
            }
            set
            {
                programborntime = value;
            }
        }

        private string programchangetime;
        public string ProgramChangeTime
        {
            get
            {
                return programchangetime;
            }
            set
            {
                programchangetime = value;
            }
        }*/

        private int devicecount;
        public int DeviceCount
        {
            get
            {
                return devicecount;
            }
            set
            {
                devicecount = value;
            }
        }

        private int runcount;
        public int RunCount
        {
            get
            {
                return runcount;
            }
            set
            {
                runcount = value;
            }
        }

        private string[] dependencyfiles;
        public string[] DependencyFiles
        {
            get
            {
                return dependencyfiles;
            }
            set
            {
                dependencyfiles = value;
            }
        }

        #endregion PrefetchParameters

        #region PrefetchConstructors
        private Prefetch(string version, string name, string pathhash, DateTime[] prefetchaccesstime, string[] dependencyfiles, int dependencycount, string path, int devicecount, int runcount)
        {
            this.Version = version;
            this.Name = name;
            this.PathHash = pathhash;
            this.PrefetchAccessTime = prefetchaccesstime;
            this.DependencyFiles = dependencyfiles;
            this.DependencyCount = dependencycount;
            this.Path = path;
            this.DeviceCount = devicecount;
            this.RunCount = runcount;
        }
        
        #endregion PrefetchConstructors

        #region PrivatePrefetchMethods

        // Check that file is actually a Prefetch File
        private static bool checkPfMagic(byte[] bytes)
        {
            // Instantiate byte array
            byte[] magicBytes = new byte[0x04];

            // Create sub-array "magicBytes" from byte array "bytes"
            Array.Copy(bytes, 0x04, magicBytes, 0, magicBytes.Length);

            // Check to see if magicBytes are equal to the Prefetch Magic Value
            if (Encoding.ASCII.GetString(magicBytes) == Magic)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        // Get Prefetch Path Hash Value
        private static string getPfPathHash(byte[] bytes)
        {
            // Instantiate byte array
            byte[] pfHashBytes = new byte[0x04];

            // Create sub-array "pfHashBytes" from byte array "bytes"
            Array.Copy(bytes, 0x4C, pfHashBytes, 0, pfHashBytes.Length);
            
            // Reverse Little Endian bytes
            Array.Reverse(pfHashBytes);
            
            // Return string representing Prefetch Path Hash
            return BitConverter.ToString(pfHashBytes).Replace("-", "");
        }

        // 
        private static DateTime[] getPfAccessTime(byte ver, byte[] bytes)
        {
            // Instantiate a null byte array
            byte[] pfAccessTimeBytes = null;

            // Instantiate a List of DateTime Objects
            List<DateTime> pfAccessTimeList = new List<DateTime>();
            
            // Zero out counter
            int counter = 0;

            // Check Prefetch version
            switch (ver)
            {
                // Windows 8 Version
                case 0x1A:
                    pfAccessTimeBytes = new byte[0x40];
                    Array.Copy(bytes, 0x80, pfAccessTimeBytes, 0, 0x40);
                    counter = 64;
                    break;
                // Windows 7 Version
                case 0x17:
                    pfAccessTimeBytes = new byte[0x08];
                    Array.Copy(bytes, 0x80, pfAccessTimeBytes, 0, 0x08);
                    counter = 8;
                    break;
                // Windows XP Version
                case 0x11:
                    pfAccessTimeBytes = new byte[0x08];
                    Array.Copy(bytes, 0x78, pfAccessTimeBytes, 0, 0x08);
                    counter = 8;
                    break;
            }

            for (int i = 0; i < counter; i += 8)
            {
                long winFileTime = BitConverter.ToInt64(pfAccessTimeBytes, i);
                DateTime dt = DateTime.FromFileTimeUtc(winFileTime);
                if ((ver == 0x1A) && (dt.ToString() == "1/1/1601 12:00:00 AM"))
                {
                    break;
                }

                pfAccessTimeList.Add(dt);

            }

            return pfAccessTimeList.ToArray();
        }

        //
        private static byte[] getPfDependencySection(byte[] bytes)
        {

            int dependencyOffsetValue = BitConverter.ToInt32((bytes.Skip(0x64).Take(0x04).ToArray()), 0);
            int dependencyLengthValue = BitConverter.ToInt32((bytes.Skip(0x68).Take(0x04).ToArray()), 0);
            return bytes.Skip(dependencyOffsetValue).Take(dependencyLengthValue).ToArray();

        }

        private static string[] getPfDependencies(byte[] pfDependencyBytes)
        {
            var dependencyString = Encoding.Unicode.GetString(pfDependencyBytes);
            string[] dependencyArraySplit = dependencyString.Split(new string[] { "\\DEVICE\\" }, StringSplitOptions.RemoveEmptyEntries);
            string[] dependencyArray = new string[dependencyArraySplit.Count()];
            for (int i = 0; i < dependencyArraySplit.Count(); i++)
            {
                string dependency = dependencyArraySplit[i].Replace("HARDDISKVOLUME1", "\\DEVICE\\HARDDISKVOLUME1").Replace("\0", string.Empty);
                dependencyArray[i] = dependency;
            }
            return dependencyArray;
        }

        //Application Path
        private static string getPfPath(string appName, string[] dependencyArray, int dependencyCount)
        {
            for (int i = 0; i < dependencyCount; i++)
            {
                if (dependencyArray[i].Contains(appName))
                {
                    return dependencyArray[i];
                }
            }
            return null;
        }

        // Get int representing the number of devices associated with prefetch record
        private static int getPfDeviceCount(byte[] bytes)
        {
            // Instantiate byte array
            byte[] deviceCountBytes = new byte[0x04];

            // Create sub-array "deviceCountBytes" from byte array "bytes"
            Array.Copy(bytes, 0x70, deviceCountBytes, 0, deviceCountBytes.Length);
            
            // Return int representing the number of devices associated with this application
            return BitConverter.ToInt32(deviceCountBytes, 0);
        }

        // Get application run count
        private static int getPfRunCount(byte ver, byte[] bytes)
        {
            // Instantiate byte array
            byte[] runCountBytes = new byte[0x04];

            // Check version and put correct bytes in runCountBytes byte array
            switch (ver)
            {
                case 0x1A:
                    //runCountBytes = bytes.Skip(0xD0).Take(0x04).ToArray();
                    Array.Copy(bytes, 0xD0, runCountBytes, 0, runCountBytes.Length);
                    break;
                case 0x17:
                    //runCountBytes = bytes.Skip(0x98).Take(0x04).ToArray();
                    Array.Copy(bytes, 0x98, runCountBytes, 0, runCountBytes.Length);
                    break;
                case 0x11:
                    //runCountBytes = bytes.Skip(0x90).Take(0x04).ToArray();
                    Array.Copy(bytes, 0x90, runCountBytes, 0, runCountBytes.Length);
                    break;
            }

            // Return run count as int
            return BitConverter.ToInt32(runCountBytes, 0);
        }

        #endregion PrivatePrefetchMethods

        #region StaticPrefetchMethods

        public static Prefetch Get(string volume, FileStream streamToRead, byte[] MFT, string prefetchPath)
        {

            // Get bytes for specific Prefetch file
            byte[] fileBytes = MFTRecord.getFile(volume, streamToRead, MFT, prefetchPath).ToArray();
            
            // Check for Prefetch Magic Number (Value) SCCA at offset 0x04 - 0x07
            if (checkPfMagic(fileBytes))
            {

                // Check Prefetch file for version (0x1A = Win 8, 0x17 = Win 7, 0x11 = Win XP)
                byte pfVersion = fileBytes[0];

                string appName = null;
                string[] dependencyArray = null;
                int dependencyCount = 0;

                appName = System.Text.Encoding.Unicode.GetString((fileBytes.Skip(0x10).Take(0x3C).ToArray())).TrimEnd('\0');
                dependencyArray = getPfDependencies(getPfDependencySection(fileBytes));

                Prefetch prefetch = new Prefetch(
                    Enum.GetName(typeof(PREFETCH_VERSION), pfVersion),
                    appName,
                    getPfPathHash(fileBytes),
                    getPfAccessTime(pfVersion, fileBytes),
                    dependencyArray,
                    dependencyArray.Length,
                    getPfPath(appName, dependencyArray, dependencyCount),
                    getPfDeviceCount(fileBytes),
                    getPfRunCount(pfVersion, fileBytes)
                );

                return prefetch;

            }

            else
            {
                return null;
            }
        }

        #endregion StaticPrefetchMethods

    }
}
