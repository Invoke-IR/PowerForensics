using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetExplorerTypedPathCommand

    /// <summary> 
    /// This class implements the Get-ExplorerTypedPath cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ExplorerTypedPath")]
    public class GetExplorerTypedPathCommand : PSCmdlet
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
            WriteObject(TypedPaths.GetInstances(hivePath), true);
        } 

        #endregion Cmdlet Overrides
    }

    #endregion GetExplorerTypedPathCommand
}
