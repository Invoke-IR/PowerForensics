using System;
using System.Management.Automation;
using PowerForensics.Registry;

namespace PowerForensics.Cmdlets
{
    #region GetRegistryKeyCommand
    
    /// <summary> 
    /// This class implements the Get-RegistryKey cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicRegistryKey", DefaultParameterSetName = "Default")]
    public class GetRegistryKeyCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// 
        /// </summary> 
        [Alias("Path")]
        [Parameter(Mandatory = true)]
        public string HivePath
        {
            get { return path; }
            set { path = value; }
        }
        private string path;

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter(ParameterSetName = "ByKey")]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string key;

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter(Mandatory = true, ParameterSetName = "Recursive")]
        public SwitchParameter Recurse
        {
            get { return recurse; }
            set { recurse = value; }
        }
        private SwitchParameter recurse;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a FileRecord objects that
        /// corresponds to the file(s) that is/are specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            if (recurse)
            {
                WriteObject(NamedKey.GetInstancesRecurse(path));
            }
            else
            {
                if (!(MyInvocation.BoundParameters.ContainsKey("Key")))
                {
                    key = null;
                }

                WriteObject(NamedKey.GetInstances(path, key), true);
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetRegistryKeyCommand
}
