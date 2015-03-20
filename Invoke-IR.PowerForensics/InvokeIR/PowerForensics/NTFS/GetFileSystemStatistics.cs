using System;
using System.Management.Automation;
using System.Text.RegularExpressions;


namespace InvokeIR.PowerForensics.NTFS
{

    #region GetFSStatCommand
    /// <summary> 
    /// This class implements the Get-FSStat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "FileSystemStat", SupportsShouldProcess = true)]
    public class GetFileSystemStatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the NTFSVolumeData object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
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

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volumeName))
            {

                volumeName = @"\\.\" + volumeName + ":";

            }

            WriteDebug("VolumeName: " + volumeName);

            IntPtr hVolume = Win32.getHandle(volumeName);

            WriteObject(NTFS.NTFSVolumeData.Get(hVolume));

            Win32.CloseHandle(hVolume);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetFSstatCommand


}
