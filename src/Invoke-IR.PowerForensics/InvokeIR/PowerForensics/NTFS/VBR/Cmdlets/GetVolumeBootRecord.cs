using System;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS.VBR
{

    #region GetVolumeBootRecordCommand
    /// <summary> 
    /// This class implements the Get-GetVolumeBootRecord cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "VolumeBootRecord")]
    public class GetVolumeBootRecordCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the DriveName for the 
        /// Partition Table that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, Position = 0)]
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

             WriteObject(new NTFS_VBR(bootbytes));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetVolumeBootRecordCommand class. 

    #endregion GetVolumeBootRecordCommand

}
