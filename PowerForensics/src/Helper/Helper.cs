using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace PowerForensics
{
    public static class Helper
    {
        #region Helper Functions

        internal static string getVolumeName(ref string volume)
        {
            if (volume == null)
            {
                volume = System.IO.Directory.GetCurrentDirectory().Split('\\')[0];
            }

            if (!(volume.Contains(@"\\.\")))
            {
                volume = @"\\.\" + volume;
            }

            return volume;
        }

        internal static string GetVolumeFromPath(string path)
        {
            return "\\\\.\\" + path.Split('\\')[0];
        }

        internal static string GetVolumeLetter(string volume)
        {
            return volume.Split('\\')[3];
        }

        internal static string GetSecurityDescriptor(byte[] bytes)
        {
            IntPtr ptrSid;
            string sidString;

            if(!NativeMethods.ConvertSidToStringSid(bytes, out ptrSid))
            {
                int HRESULT = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(HRESULT);
            }

            try
            {
                sidString = Marshal.PtrToStringAnsi(ptrSid);
            }
            finally
            {
                NativeMethods.LocalFree(ptrSid);
            }

            return sidString;
        }

        internal static FileStream getFileStream(string fileName)
        {
            if (!(fileName.Contains(@"\\.\PHYSICALDRIVE")))
            {
                getVolumeName(ref fileName);
            }

            // Get Handle to specified Volume/File/Directory
            SafeFileHandle hDevice = NativeMethods.CreateFile(
                fileName: fileName,
                fileAccess: FileAccess.Read,
                fileShare: FileShare.Write | FileShare.Read | FileShare.Delete,
                securityAttributes: IntPtr.Zero,
                creationDisposition: FileMode.Open,
                flags: NativeMethods.FILE_FLAG_BACKUP_SEMANTICS,
                template: IntPtr.Zero);

            // Check if handle is valid
            if (hDevice.IsInvalid)
            {
                int HRESULT = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(HRESULT);
                //throw new Exception(@"Invalid handle to Volume/Drive returned. PowerShell must be run as Administrator to get a device handle.");
            }

            // Return a FileStream to read from the specified handle
            return new FileStream(hDevice, FileAccess.Read);
        }

        internal static byte[] readDrive(string device, ulong offset, ulong sizeToRead)
        {
            // Create a FileStream to read from hDrive
            using (FileStream streamToRead = getFileStream(device))
            {
                return readDrive(streamToRead, offset, sizeToRead);
            }
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

        internal static byte[] GetSubArray(byte[] InputBytes, int offset, int length)
        {
            byte[] outputBytes = new byte[length];
            Array.Copy(InputBytes, offset, outputBytes, 0x00, outputBytes.Length);
            return outputBytes;
        }

        internal static void ApplyFixup(ref byte[] bytes, int offset)
        {
            // Take UpdateSequence into account
            ushort usoffset = BitConverter.ToUInt16(bytes, 0x04);
            ushort ussize = BitConverter.ToUInt16(bytes, 0x06);

            if (ussize != 0)
            {
                ushort UpdateSequenceNumber = BitConverter.ToUInt16(bytes, usoffset + offset);
                byte[] UpdateSequenceArray = Helper.GetSubArray(bytes, (usoffset + 2 + offset), (2 * ussize));

                bytes[0x1FE + offset] = UpdateSequenceArray[0x00];
                bytes[0x1FF + offset] = UpdateSequenceArray[0x01];
                bytes[0x3FE + offset] = UpdateSequenceArray[0x02];
                bytes[0x3FF + offset] = UpdateSequenceArray[0x03];
                bytes[0x5FE + offset] = UpdateSequenceArray[0x04];
                bytes[0x5FF + offset] = UpdateSequenceArray[0x05];
                bytes[0x7FE + offset] = UpdateSequenceArray[0x06];
                bytes[0x7FF + offset] = UpdateSequenceArray[0x07];
                bytes[0x9FE + offset] = UpdateSequenceArray[0x08];
                bytes[0x9FF + offset] = UpdateSequenceArray[0x09];
                bytes[0xBFE + offset] = UpdateSequenceArray[0x0A];
                bytes[0xBFF + offset] = UpdateSequenceArray[0x0B];
                bytes[0xDFE + offset] = UpdateSequenceArray[0x0C];
                bytes[0xDFF + offset] = UpdateSequenceArray[0x0D];
                bytes[0xFFE + offset] = UpdateSequenceArray[0x0E];
                bytes[0xFFF + offset] = UpdateSequenceArray[0x0F];
            }
        }

        internal static string FromRot13(string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = (int)array[i];

                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }

        #endregion Helper Functions
    }
}

