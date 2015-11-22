using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetNetworkListCommand

    /// <summary> 
    /// This class implements the Get-NetworkList cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "NetworkList")]
    public class GetNetworkListCommand : PSCmdlet
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
                WriteObject(NetworkList.GetInstances(hivePath), true);
            }
            else
            {
                WriteObject(NetworkList.GetInstances(), true);
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetNetworkListCommand
}
