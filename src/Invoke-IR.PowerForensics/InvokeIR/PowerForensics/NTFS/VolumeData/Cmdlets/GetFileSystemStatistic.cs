using System;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetFileSystemStatisticCommand
    /// <summary> 
    /// This class implements the Get-FileSystemStatistic cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "FileSystemStatistic", SupportsShouldProcess = true)]
    public class GetFileSystemStatisticCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the NTFSVolumeData object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true, Position = 0)]
        public string VolumeName
        {
            get { return volumeName; }
            set { volumeName = value; }
        }
        private string volumeName;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a NTFSVolumeData objects that
        /// corresponds to the VolumeName that is specified.
        /// </summary> 

        protected override void ProcessRecord()
        {

            // Get correct volume name from user input
            NativeMethods.getVolumeName(ref volumeName);

            // Get handle to Logical Volume
            IntPtr hVolume = NativeMethods.getHandle(volumeName);

            WriteObject(new VolumeData(hVolume));

            NativeMethods.CloseHandle(hVolume);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFileSystemStatisticCommand class. 

    #endregion GetFileSystemStatisticCommand

}
