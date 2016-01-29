using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetVolumeNameCommand

    /// <summary> 
    /// This class implements the Get-VolumeName cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicVolumeName", DefaultParameterSetName = "ByVolume")]
    public class GetVolumeNameCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the the name of the target volume.
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
        [ValidatePattern(@"^(\\\\\.\\)?[A-Zaz]:$")]
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
                    WriteObject(Ntfs.VolumeName.Get(volume));
                    break;
                case "ByPath":
                    WriteObject(Ntfs.VolumeName.GetByPath(path));
                    break;
            }

        } 

        #endregion Cmdlet Overrides
    }

    #endregion GetVolumeNameCommand
}
