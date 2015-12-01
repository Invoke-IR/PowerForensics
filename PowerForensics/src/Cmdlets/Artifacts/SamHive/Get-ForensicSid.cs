using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetSidCommand

    /// <summary> 
    /// This class implements the Get-Sid cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "Sid")]
    public class GetSidCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the the path of the Registry Hive to parse.
        /// </summary> 
        [Alias("Path")]
        [Parameter(Position = 0)]
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
            if (MyInvocation.BoundParameters.ContainsKey("HivePath"))
            {
                WriteObject(Sid.Get(hivePath));
            }
            else
            {
                WriteObject(Sid.Get());
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetSidCommand
}
