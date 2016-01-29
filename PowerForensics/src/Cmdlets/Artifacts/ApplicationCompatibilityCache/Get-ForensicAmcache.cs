using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetAmcacheCommand

    /// <summary> 
    /// This class implements the Get-Amcache cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicAmcache", DefaultParameterSetName = "ByVolume")]
    public class GetAmcacheCommand : PSCmdlet
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
        /// This parameter provides the the path of the Registry Hive to parse.
        /// </summary> 
        [Alias("Path")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath")]
        public string HivePath
        {
            get { return hivePath; }
            set { hivePath = value; }
        }
        private string hivePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method reads the raw contents of the Amcache.hve into memory and parses its
        /// values to create/output AppCompat Objects.
        /// </summary>  
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(Amcache.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(Amcache.GetInstancesByPath(hivePath), true);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetAmcacheCommand
}
