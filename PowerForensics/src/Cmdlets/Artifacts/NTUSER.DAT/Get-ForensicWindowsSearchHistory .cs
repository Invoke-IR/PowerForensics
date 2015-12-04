using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetWindowsSearchHistoryCommand

    /// <summary> 
    /// This class implements the Get-WindowsSearchHistory  cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicWindowsSearchHistory")]
    public class GetWindowsSearchHistoryCommand : PSCmdlet
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
            WriteObject(WordWheelQuery.GetInstances(hivePath), true);
        }

    #endregion Cmdlet Overrides
}

    #endregion GetWindowsSearchHistoryCommand
}
