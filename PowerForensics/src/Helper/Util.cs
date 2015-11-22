using System;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace PowerForensics
{
    public static class Util
    {
        #region Helper Functions

        internal static void checkAdmin()
        {
            bool admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

            if (!(admin))
            {
                throw new Exception("This cmdlet requires administrator privileges. Start Windows PowerShell with the \"Run as administrator\" option and try the command again.");
            }
        }

        internal static string getDriveName(string drive)
        {
            Regex lettersOnly = new Regex(@"\\\\\.\\PHYSICALDRIVE\d");

            if (!(lettersOnly.IsMatch(drive.ToUpper())))
            {
                throw new Exception("Provided Drive Name is not acceptable.");
            }

            return drive;
        }

        internal static string getVolumeName(ref string volume)
        {
            if (volume == null)
            {
                volume = System.IO.Directory.GetCurrentDirectory().Split(':')[0];
            }

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");
            Regex lettercolon = new Regex(@"^[a-zA-Z]:$");
            Regex volLetter = new Regex(@"[a-zA-Z]:\\");
            Regex uncPath = new Regex(@"\\\\\.\\[a-zA-Z]:");
            Regex vsc = new Regex(@"\\\\\?\\GLOBALROOT\\DEVICE\\HARDDISKVOLUMESHADOWCOPY\d+$");
            Regex physicalDrive = new Regex(@"\\\\\.\\PHYSICALDRIVE\d");

            if (lettersOnly.IsMatch(volume))
            {
                volume = @"\\.\" + volume + ":";
            }
            else if (lettercolon.IsMatch(volume))
            {
                volume = @"\\.\" + volume;
            } 
            else if (volLetter.IsMatch(volume))
            {
                volume = @"\\.\" + volume.TrimEnd('\\');
            }
            else if (uncPath.IsMatch(volume))
            {

            }
            else if (vsc.IsMatch(volume.ToUpper()))
            {

            }
            else if (physicalDrive.IsMatch(volume.ToUpper()))
            {

            }
            else
            {

                throw new Exception(@"The VolumeName value is not in the correct format. The following formats are valid: \.\C:, C:, or C.");
            }

            return volume;

        }

        internal static IntPtr getHandle(string FileName)
        {
            Regex physicalDrive = new Regex(@"\\\\\.\\PHYSICALDRIVE\d");
            if (physicalDrive.IsMatch(FileName))
            {
                FileName = FileName.TrimEnd(':');
            }

            // Get Handle to specified Volume/File/Directory
            IntPtr hDrive = NativeMethods.CreateFile(
                fileName: FileName,
                fileAccess: FileAccess.Read,
                fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                securityAttributes: IntPtr.Zero,
                creationDisposition: FileMode.Open,
                flags: NativeMethods.FILE_FLAG_BACKUP_SEMANTICS,
                template: IntPtr.Zero);

            // Check if handle is valid
            if (hDrive.ToInt32() == NativeMethods.INVALID_HANDLE_VALUE)
            {
                // If handle is not valid throw an error
                throw new Exception("Invalid handle to Volume/Drive returned");
            }

            // Return handle
            return hDrive;

        }

        internal static FileStream getFileStream(IntPtr hVolume)
        {
            // Return a FileStream to read from the specified handle
            return new FileStream(hVolume, FileAccess.Read);
        }

        internal static byte[] readDrive(FileStream streamToRead, ulong offset, ulong sizeToRead)
        {

            // Bytes must be read by sector
            //if ((sizeToRead < 1)) throw new System.ArgumentException("Size parameter cannot be null or 0 or less than 0!");
            if (((sizeToRead % 512) != 0)) throw new System.ArgumentException("Size parameter must be divisible by 512");
            if (((offset % 512) != 0)) throw new System.ArgumentException("Offset parameter must be divisible by 512");

            // Set offset to begin reading from the drive
            streamToRead.Position = (long)offset;

            // Create a byte array to read into
            byte[] buf = new byte[sizeToRead];

            // Read buf.Length bytes (sizeToRead) from offset 
            try
            {
                Int32 bytesRead = streamToRead.Read(buf, 0, buf.Length);

                if (bytesRead != buf.Length)
                {
                    if (bytesRead > buf.Length)
                    {
                        throw new Exception("The readDrive method read more bytes from disk than expected.");
                    }
                    /*else if (bytesRead < buf.Length)
                    {
                        throw new Exception("The readDrive method read less bytes from disk than expected.");
                    }*/
                }

            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("The readDrive method experienced an ArgumentNullException.");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException("The readDrive method experienced an ArgumentOutOfRangeException.");
            }
            catch (EndOfStreamException)
            {
                throw new EndOfStreamException("The readDrive method experienced an EndOfStreamException.");
            }
            catch (IOException)
            {
                throw new IOException("The readDrive method experienced an IOException.");
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The readDrive method experienced an ArgumentException");
            }
            catch (ObjectDisposedException)
            {
                throw new ObjectDisposedException("The readDrive method experienced an ObjectDisposedException");
            }

            return buf;

        }

        internal static DateTime FromUnixTime(uint unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        internal static string GetVolumeFromPath(string path)
        {
            return "\\\\.\\" + path.Split('\\')[0];
        }

        internal static string GetVolumeLetter(string volume)
        {
            return volume.Split('\\')[3];
        }

        internal static byte[] GetSubArray(byte[] InputBytes, uint offset, uint length)
        {
            byte[] outputBytes = new byte[length];
            Array.Copy(InputBytes, offset, outputBytes, 0, outputBytes.Length);
            return outputBytes;
        }

        #endregion Helper Functions
    }
}

