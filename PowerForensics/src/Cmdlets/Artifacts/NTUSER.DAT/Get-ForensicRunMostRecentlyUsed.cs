using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetRunMostRecentlyUsedCommand

    /// <summary> 
    /// This class implements the Get-RunMostRecentlyUsed  cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicRunMostRecentlyUsed")]
    public class GetRunMostRecentlyUsedCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the the path of the Registry Hive to parse.
        /// </summary> 
        [Alias("Path")]
        [Parameter(Mandatory = true, Position = 0)]
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
        protected override void ProcessRecord()
        {
            WriteObject(RunMRU.GetInstances(hivePath), true);
        }

    #endregion Cmdlet Overrides
}

    #endregion GetRunMostRecentlyUsedCommand
}
