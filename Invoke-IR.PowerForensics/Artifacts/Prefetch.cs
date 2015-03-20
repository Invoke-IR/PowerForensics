using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using InvokeIR.PowerForensics.NTFS.MFT;

namespace InvokeIR.PowerForensics.Artifacts.Prefetch
{
    public class Prefetch
    {

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
        private static bool checkPfMagic(List<byte> fileBytes)
        {
            if ((fileBytes[0x04] == 83) & (fileBytes[0x05] == 67) & (fileBytes[0x06] == 67) & (fileBytes[0x07] == 65))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string getPfPathHash(List<byte> bytes)
        {
            byte[] pfHashBytes = bytes.Skip(0x4C).Take(0x04).ToArray();
            Array.Reverse(pfHashBytes);
            return BitConverter.ToString(pfHashBytes).Replace("-", "");
        }

        private static DateTime[] getPfAccessTime(string ver, List<byte> bytes)
        {

            byte[] pfAccessTimeBytes = null;
            List<DateTime> pfAccessTimeList = new List<DateTime>();
            int counter = 0;

            switch (ver)
            {
                case "Windows 8":
                    pfAccessTimeBytes = bytes.Skip(0x80).Take(0x40).ToArray();
                    counter = 64;
                    break;
                case "Windows 7":
                    pfAccessTimeBytes = bytes.Skip(0x80).Take(0x08).ToArray();
                    counter = 8;
                    break;
                case "Windows XP":
                    pfAccessTimeBytes = bytes.Skip(0x78).Take(0x08).ToArray();
                    counter = 8;
                    break;
            }

            for (int i = 0; i < counter; i += 8)
            {
                long winFileTime = BitConverter.ToInt64(pfAccessTimeBytes, i);
                DateTime dt = DateTime.FromFileTimeUtc(winFileTime);
                if ((ver == "Windows 8") && (dt.ToString() == "1/1/1601 12:00:00 AM"))
                {
                    break;
                }

                pfAccessTimeList.Add(dt);

            }

            return pfAccessTimeList.ToArray();
        }

        private static byte[] getPfDependencySection(string ver, List<byte> bytes)
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

        private static int getPfDeviceCount(string ver, List<byte> bytes)
        {
            return BitConverter.ToInt32((bytes.Skip(0x70).Take(0x04).ToArray()), 0);
        }

        private static int getPfRunCount(string ver, List<byte> bytes)
        {
            byte[] runCountBytes = null;

            switch (ver)
            {
                case "Windows 8":
                    runCountBytes = bytes.Skip(0xD0).Take(0x04).ToArray();
                    break;
                case "Windows 7":
                    runCountBytes = bytes.Skip(0x98).Take(0x04).ToArray();
                    break;
                case "Windows XP":
                    runCountBytes = bytes.Skip(0x90).Take(0x04).ToArray();
                    break;
            }

            return BitConverter.ToInt32(runCountBytes, 0);
        }

        #endregion PrivatePrefetchMethods

        #region StaticPrefetchMethods
        public static Prefetch Get(string volume, FileStream streamToRead, byte[] MFT, string prefetchPath)
        {

            List<byte> fileBytes = MFTRecord.getFile(volume, streamToRead, MFT, prefetchPath);

            // Check for Prefetch Magic Number (Value) SCCA at offset 0x04 - 0x07

            if (checkPfMagic(fileBytes))
            {
                // Check Prefetch file for version (1A = Win 8, 17 = Win 7, 11 = Win XP)
                byte pfVersion = fileBytes[0];

                string ver = null;
                string appName = null;
                DateTime[] pfAccessTimeArray = new DateTime[8];
                string[] dependencyArray = null;
                int dependencyCount = 0;

                switch (pfVersion)
                {
                    case 26:
                        ver = "Windows 8";
                        break;
                    case 23:
                        ver = "Windows 7";
                        break;
                    case 17:
                        ver = "Windows XP";
                        break;
                }

                appName = System.Text.Encoding.Unicode.GetString((fileBytes.Skip(0x10).Take(0x3C).ToArray())).TrimEnd('\0');
                dependencyArray = getPfDependencies(getPfDependencySection(ver, fileBytes));
                dependencyCount = dependencyArray.Length;

                Prefetch prefetch = new Prefetch(
                    ver,
                    appName,
                    getPfPathHash(fileBytes),
                    getPfAccessTime(ver, fileBytes),
                    dependencyArray,
                    dependencyCount,
                    getPfPath(appName, dependencyArray, dependencyCount),
                    getPfDeviceCount(ver, fileBytes),
                    getPfRunCount(ver, fileBytes)
                );

                return prefetch;
            }
            else
            {
                return null;
            }
        }


        #endregion StaticPrefetchMethods

        #region GetPrefetchCommand
        /// <summary> 
        /// This class implements the Get-Prefetch cmdlet. 
        /// </summary> 

        [Cmdlet(VerbsCommon.Get, "Prefetch", DefaultParameterSetName = "One")]
        public class GetPrefetchCommand : PSCmdlet
        {
            #region Parameters

            /// <summary> 
            /// This parameter provides the list of process names on  
            /// which the Stop-Proc cmdlet will work. 
            /// </summary> 

            [Parameter(Mandatory = true, ParameterSetName = "Two")]
            public string FilePath
            {
                get { return filePath; }
                set { filePath = value; }
            }
            private string filePath;

            #endregion Parameters

            #region Cmdlet Overrides

            /// <summary> 
            /// The ProcessRecord method calls ManagementClass.GetInstances() 
            /// method to iterate through each BindingObject on each system specified.
            /// </summary> 
            protected override void ProcessRecord()
            {

                string volLetter = Directory.GetCurrentDirectory().Split('\\')[0];
                string volume = @"\\.\" + volLetter;

                IntPtr hVolume = Win32.getHandle(volume);
                FileStream streamToRead = Win32.getFileStream(hVolume);
                byte[] MFT = MasterFileTable.GetBytes(hVolume, streamToRead);

                if (this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
                {
                    WriteObject(Prefetch.Get(volume, streamToRead, MFT, filePath));
                }

                else
                {

                    string prefetchPath = volLetter + @"\\Windows\\Prefetch";
                    var pfFiles = System.IO.Directory.GetFiles(prefetchPath, "*.pf").ToArray();
                    foreach(var file in pfFiles)
                    {
                        WriteObject(Prefetch.Get(volume, streamToRead, MFT, file));
                    }

                }

            } // ProcessRecord 

            #endregion Cmdlet Overrides
        } // End GetProcCommand class. 
        #endregion GetPrefetchCommand

    }
}
