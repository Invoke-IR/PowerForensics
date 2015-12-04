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
        protected override void BeginProcessing()
        {
            Util.checkAdmin();
            
            if (ParameterSetName == "ByVolume")
            {
                Util.getVolumeName(ref volume);
            }
        }

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
                        WriteObject(UsnJrnlDetail.GetBytes(volume.Split('\\')[3] + "\\$Extend\\$UsnJrnl"), true);
                    }
                    else
                    {
                        WriteObject(UsnJrnlDetail.Get(volume.Split('\\')[3] + "\\$Extend\\$UsnJrnl"), true);
                    }
                    break;
                case "ByPath":
                    if (asBytes)
                    {
                        WriteObject(UsnJrnlDetail.GetBytes(path));
                    }
                    else
                    {
                        WriteObject(UsnJrnlDetail.Get(path));
                    }
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetUsnJrnlInformationCommand
}
