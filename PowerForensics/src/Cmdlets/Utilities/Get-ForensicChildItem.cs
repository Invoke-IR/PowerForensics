using System.Management.Automation;

namespace PowerForensics.Ntfs
{
    #region GetChildItemCommand
    /// <summary> 
    /// This class implements the Get-ChildItem cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicChildItem")]
    public class GetChildItemCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Alias("FullName")]
        [Parameter(Position = 0)]
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
        protected override void BeginProcessing()
        {
            if (!(this.MyInvocation.BoundParameters.ContainsKey("Path")))
            {
                path = this.SessionState.PSVariable.GetValue("pwd").ToString();
            }
        }

        /// <summary> 
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            try
            {
                foreach (IndexEntry index in IndexEntry.GetInstances(path))
                {
                    WriteObject(index);
                }
            }
            catch
            {
                WriteObject(null);
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetChildItemCommand
}
