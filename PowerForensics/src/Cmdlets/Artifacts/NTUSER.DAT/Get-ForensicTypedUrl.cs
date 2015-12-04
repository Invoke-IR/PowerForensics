using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetTypedUrlCommand

    /// <summary> 
    /// This class implements the Get-TypedUrl cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicTypedUrl")]
    public class GetTypedUrlCommand : PSCmdlet
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
            WriteObject(TypedUrls.GetInstances(hivePath), true);
        } 

        #endregion Cmdlet Overrides
    }

    #endregion GetTypedUrlCommand
}
