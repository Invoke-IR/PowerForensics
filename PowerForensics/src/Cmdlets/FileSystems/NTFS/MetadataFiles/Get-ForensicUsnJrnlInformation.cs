using System.Management.Automation;
using PowerForensics.Ntfs;

namespace PowerForensics.Cmdlets
{
    #region GetUsnJrnlInformationCommand

    /// <summary> 
    /// This class implements the Get-UsnJrnlInformation cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicUsnJrnlInformation", DefaultParameterSetName = "ByVolume")]
    public class GetUsnJrnlInformationCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the the name of the target volume.
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
        [ValidatePattern(@"^(\\\\\.\\)?([A-Za-z]:|PHYSICALDRIVE\d)$")]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// 
        /// </summary> 
        [Alias("FullName")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath", ValueFromPipelineByPropertyName = true)]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string path;

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter()]
        public SwitchParameter AsBytes
        {
            get { return asBytes; }
            set { asBytes = value; }
        }
        private SwitchParameter asBytes;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// 
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    if (asBytes)
                    {
                        WriteObject(UsnJrnlInformation.GetBytes(volume), true);
                    }
                    else
                    {
                        WriteObject(UsnJrnlInformation.Get(volume), true);
                    }
                    break;
                case "ByPath":
                    if (asBytes)
                    {
                        WriteObject(UsnJrnlInformation.GetBytesByPath(path));
                    }
                    else
                    {
                        WriteObject(UsnJrnlInformation.GetByPath(path));
                    }
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetUsnJrnlInformationCommand
}
