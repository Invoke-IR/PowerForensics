using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetShimcacheCommand

    /// <summary> 
    /// This class implements the Get-Shimcache cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicShimcache", DefaultParameterSetName = "ByVolume")]
    public class GetShimcacheCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
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
        /// The ProcessRecord method reads the raw contents of the Amcache.hve into memory and parses its
        /// values to create/output AppCompat Objects.
        /// </summary>  
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(Shimcache.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(Shimcache.GetInstancesByPath(hivePath), true);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetShimcacheCommand
}
