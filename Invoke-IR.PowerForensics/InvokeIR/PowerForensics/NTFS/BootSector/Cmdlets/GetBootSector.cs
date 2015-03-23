using System;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS.BootSector
{

    #region GetBootSectorCommand
    /// <summary> 
    /// This class implements the Get-BootFile cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "BootSector")]
    public class GetBootSectorCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the DriveName for the 
        /// Partition Table that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";

            }

            IntPtr hVolume = NativeMethods.getHandle(volume);
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            byte[] bootbytes = NativeMethods.readDrive(streamToRead, 0, 512);

             WriteObject(new NTFS_BPB(bootbytes));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetProcCommand class. 

    #endregion GetBootSectorCommand

}
