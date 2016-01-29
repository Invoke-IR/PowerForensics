using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetRecentFileCacheCommand

    /// <summary> 
    /// This class implements the Get-RecentFileCache cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicRecentFileCache", DefaultParameterSetName = "ByVolume")]
    public class GetRecentFileCacheCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// 
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
        /// This parameter provides the the path of the RecentFileCache.bcf file to parse.
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
                    WriteObject(RecentFileCache.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(RecentFileCache.GetInstancesByPath(path), true);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetRecentFileCacheCommand
}
