using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetTypedPathCommand

    /// <summary> 
    /// This class implements the Get-TypedPath cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "TypedPath")]
    public class GetTypedPathCommand : PSCmdlet
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

    #endregion GetTypedPathCommand
}
