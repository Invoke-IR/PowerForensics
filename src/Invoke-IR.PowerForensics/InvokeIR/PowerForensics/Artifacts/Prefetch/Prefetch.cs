using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Artifacts
{
    #region PrefetchClass

    public class Prefetch
    {
        #region Constants

        const string PREFETCH_MAGIC = "SCCA";

        #endregion Constants

        #region Enums

        enum PREFETCH_VERSION
        {
            WINDOWS_8 = 0x1A,
            WINDOWS_7 = 0x17,
            WINDOWS_XP = 0x11
        }

        #endregion Enums

        #region Parameters

        public readonly string Version;
        public readonly string Name;
        public readonly string Path;
//        public string MD5
        public readonly string PathHash;
        public readonly int DependencyCount;
        public readonly DateTime[] PrefetchAccessTime;
//        public readonly DateTime PrefetchBornTime;
//        public readonly string ProgramBornTime;
//        public readonly string ProgramChangeTime
        public readonly int DeviceCount;
        public readonly int RunCount;
        public readonly string[] DependencyFiles;

        #endregion Parameters

        #region Constructors

        private Prefetch(byte[] fileBytes)
        {
            // Instantiate byte array
            byte[] pfMagic = new byte[0x04];

            // Create sub-array "pfMagic" from byte array "fileBytes"
            Array.Copy(fileBytes, 0x04, pfMagic, 0x00, pfMagic.Length);

            // Check for Prefetch Magic Number (Value) SCCA at offset 0x04 - 0x07
            if (Encoding.ASCII.GetString(pfMagic) == PREFETCH_MAGIC)
            {
                // Check Prefetch file for version (0x1A = Win 8, 0x17 = Win 7, 0x11 = Win XP)
                string pfVersion = Enum.GetName(typeof(PREFETCH_VERSION), fileBytes[0]);


                //// Get Prefetch Path Hash Value ////
                // Instantiate byte array
                byte[] pfHashBytes = new byte[0x04];
                // Create sub-array "pfHashBytes" from byte array "bytes"
                Array.Copy(fileBytes, 0x4C, pfHashBytes, 0, pfHashBytes.Length);
                // Reverse Little Endian bytes
                Array.Reverse(pfHashBytes);
                // Return string representing Prefetch Path Hash
                string pfPathHash = BitConverter.ToString(pfHashBytes).Replace("-", "");


                // Get Prefetch Last Accessed Time Array //
                // Instantiate a null byte array
                byte[] pfAccessTimeBytes = null;
                // Instantiate a List of DateTime Objects
                List<DateTime> pfAccessTimeList = new List<DateTime>();
                // Zero out counter
                int counter = 0;
                // Check Prefetch version
                switch (pfVersion)
                {
                    // Windows 8 Version
                    case "WINDOWS_8":
                        pfAccessTimeBytes = new byte[0x40];
                        Array.Copy(fileBytes, 0x80, pfAccessTimeBytes, 0, 0x40);
                        counter = 64;
                        break;
                    // Windows 7 Version
                    case "WINDOWS_7":
                        pfAccessTimeBytes = new byte[0x08];
                        Array.Copy(fileBytes, 0x80, pfAccessTimeBytes, 0, 0x08);
                        counter = 8;
                        break;
                    // Windows XP Version
                    case "WINDOWS_XP":
                        pfAccessTimeBytes = new byte[0x08];
                        Array.Copy(fileBytes, 0x78, pfAccessTimeBytes, 0, 0x08);
                        counter = 8;
                        break;
                }
                for (int i = 0; i < counter; i += 8)
                {
                    long winFileTime = BitConverter.ToInt64(pfAccessTimeBytes, i);
                    DateTime dt = DateTime.FromFileTimeUtc(winFileTime);
                    if ((pfVersion == "WINDOWS_8") && (dt.ToString() == "1/1/1601 12:00:00 AM"))
                    {
                        break;
                    }
                    pfAccessTimeList.Add(dt);
                }


                ////
                string appName = System.Text.Encoding.Unicode.GetString((fileBytes.Skip(0x10).Take(0x3C).ToArray())).TrimEnd('\0');


                //// Get Dependency Files Section ////
                int dependencyOffsetValue = BitConverter.ToInt32((fileBytes.Skip(0x64).Take(0x04).ToArray()), 0);
                int dependencyLengthValue = BitConverter.ToInt32((fileBytes.Skip(0x68).Take(0x04).ToArray()), 0);
                byte[] pfDependencyBytes = fileBytes.Skip(dependencyOffsetValue).Take(dependencyLengthValue).ToArray();


                ////
                string pfPath = null;


                ////
                var dependencyString = Encoding.Unicode.GetString(pfDependencyBytes);
                string[] dependencyArraySplit = dependencyString.Split(new string[] { "\\DEVICE\\" }, StringSplitOptions.RemoveEmptyEntries);
                string[] dependencyArray = new string[dependencyArraySplit.Count()];
                for (int i = 0; i < dependencyArraySplit.Count(); i++)
                {
                    string dependency = dependencyArraySplit[i].Replace("HARDDISKVOLUME1", "\\DEVICE\\HARDDISKVOLUME1").Replace("\0", string.Empty);
                    if(dependency.Contains(appName))
                    {
                        pfPath = dependency;
                    }
                    dependencyArray[i] = dependency;
                }


                //// Get int representing the number of devices associated with prefetch record ////
                // Instantiate byte array
                byte[] deviceCountBytes = new byte[0x04];
                // Create sub-array "deviceCountBytes" from byte array "bytes"
                Array.Copy(fileBytes, 0x70, deviceCountBytes, 0, deviceCountBytes.Length);
                // Return int representing the number of devices associated with this application
                Int32 pfDeviceCount = BitConverter.ToInt32(deviceCountBytes, 0);


                //// Get application run count ////
                // Instantiate byte array
                byte[] runCountBytes = new byte[0x04];
                // Check version and put correct bytes in runCountBytes byte array
                switch (pfVersion)
                {
                    case "WINDOWS_8":
                        Array.Copy(fileBytes, 0xD0, runCountBytes, 0, runCountBytes.Length);
                        break;
                    case "WINDOWS_7":
                        Array.Copy(fileBytes, 0x98, runCountBytes, 0, runCountBytes.Length);
                        break;
                    case "WINDOWS_XP":
                        Array.Copy(fileBytes, 0x90, runCountBytes, 0, runCountBytes.Length);
                        break;
                }
                // Return run count as int
                Int32 pfRunCount =  BitConverter.ToInt32(runCountBytes, 0);


                //
                Version = pfVersion;
                Name = appName;
                PathHash = pfPathHash;
                PrefetchAccessTime = pfAccessTimeList.ToArray();
                DependencyFiles = dependencyArray;
                DependencyCount = dependencyArray.Length;
                Path = pfPath;
                DeviceCount = pfDeviceCount;
                RunCount = pfRunCount;
            }
        }
        
        #endregion Constructors

        #region StaticMethods

        public static Prefetch Get(string filePath)
        {
            // Get volume path from filePath
            string volume = @"\\.\" + filePath.Split('\\')[0];

            // Get a handle to the volume
            IntPtr hVolume = NativeMethods.getHandle(volume);

            // Create a FileStream to read from the volume handle
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                // Get a byte array representing the Master File Table
                byte[] MFT = MasterFileTable.GetBytes(hVolume, streamToRead);

                // Get bytes for specific Prefetch file
                byte[] fileBytes = MFTRecord.getFile(volume, streamToRead, MFT, filePath).ToArray();

                // Return a Prefetch object for the Prefetch file stored at filePath
                return new Prefetch(fileBytes);
            }
        }

        public static Prefetch Get(string filePath, bool fast)
        {
            // Get bytes for specific Prefetch file
            byte[] fileBytes = null;

            try
            {
                fileBytes = File.ReadAllBytes(filePath);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("ArgumentException thrown by Prefetch.GetInstance()");
            }
            catch (PathTooLongException)
            {
                throw new PathTooLongException("PathTooLongException thrown by Prefetch.GetInstance()");
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("DirectoryNotFoundException thrown by Prefetch.GetInstance()");
            }
            catch (IOException)
            {
                throw new IOException("IOException thrown by Prefetch.GetInstance()");
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("UnauthorizedAccessException thrown by Prefetch.GetInstance()");
            }

            // Return a Prefetch object for the Prefetch file stored at filePath
            return new Prefetch(fileBytes);
        }

        public static Prefetch[] GetInstances()
        {
            // Get current volume
            string volLetter = Directory.GetCurrentDirectory().Split('\\')[0];
            string volume = @"\\.\" + volLetter;

            // Get a handle to the volume
            IntPtr hVolume = NativeMethods.getHandle(volume);

            // Create a FileStream to read from the volume handle
            using (FileStream streamToRead = NativeMethods.getFileStream(hVolume))
            {
                // Get a byte array representing the Master File Table
                byte[] MFT = MasterFileTable.GetBytes(hVolume, streamToRead);

                // Build Prefetch directory path
                string prefetchPath = volLetter + @"\\Windows\\Prefetch";

                // Check prefetchPath exists
                if (Directory.Exists(prefetchPath))
                {
                    // Get list of file in the Prefetch directory that end in the .pf extension
                    var pfFiles = System.IO.Directory.GetFiles(prefetchPath, "*.pf");
                    
                    // Instantiate an array of Prefetch objects
                    Prefetch[] pfArray = new Prefetch[pfFiles.Length];
                    
                    // Iterate through Prefetch Files
                    for (int i = 0; i < pfFiles.Length; i++)
                    {
                        // Get bytes for specific Prefetch file
                        byte[] fileBytes = MFTRecord.getFile(volume, streamToRead, MFT, pfFiles[i]).ToArray();

                        // Output the Prefetch object for the corresponding file
                        pfArray[i] = (new Prefetch(fileBytes));
                    }

                    // Return array or Prefetch objects
                    return pfArray;
                }
                else
                {
                    return null;
                }
            }
        }

        public static Prefetch[] GetInstances(bool fast)
        {
            // Get current volume
            string volLetter = Directory.GetCurrentDirectory().Split('\\')[0];
            // Build Prefetch directory path
            string prefetchPath = volLetter + @"\\Windows\\Prefetch";

            // Check prefetchPath exists
            if (Directory.Exists(prefetchPath))
            {
                // Get list of file in the Prefetch directory that end in the .pf extension
                var pfFiles = System.IO.Directory.GetFiles(prefetchPath, "*.pf");
                    
                // Instantiate an array of Prefetch objects
                Prefetch[] pfArray = new Prefetch[pfFiles.Length];
                    
                // Iterate through Prefetch Files
                for (int i = 0; i < pfFiles.Length; i++)
                {
                    // Get bytes for specific Prefetch file
                    byte[] fileBytes = null;
                    
                    try
                    {
                        fileBytes = File.ReadAllBytes(pfFiles[i]);
                    }
                    catch (ArgumentException)
                    {
                        throw new ArgumentException("ArgumentException thrown by Prefetch.GetInstance()");
                    }
                    catch(PathTooLongException)
                    {
                        throw new PathTooLongException("PathTooLongException thrown by Prefetch.GetInstance()");
                    }
                    catch (DirectoryNotFoundException)
                    {
                        throw new DirectoryNotFoundException("DirectoryNotFoundException thrown by Prefetch.GetInstance()");
                    }
                    catch (IOException)
                    {
                        throw new IOException("IOException thrown by Prefetch.GetInstance()");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new UnauthorizedAccessException("UnauthorizedAccessException thrown by Prefetch.GetInstance()");
                    }


                    // Output the Prefetch object for the corresponding file
                    pfArray[i] = (new Prefetch(fileBytes));
                }

                // Return array or Prefetch objects
                return pfArray;
            }
            else
            {
                return null;
            }
        }

        #endregion StaticMethods
    }

    #endregion PrefetchClass
}
