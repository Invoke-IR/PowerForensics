using System;
using System.Management.Automation;
using PowerForensics.Registry;

namespace PowerForensics.Cmdlets
{
    #region GetRegistryValueCommand

    /// <summary> 
    /// This class implements the Get-RegistryValue cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "RegistryValue")]
    public class GetRegistryValueCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
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
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 
        [Parameter()]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string key;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 
        [Parameter()]
        public string Value
        {
            get { return val; }
            set { val = value; }
        }
        private string val;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a FileRecord objects that
        /// corresponds to the file(s) that is/are specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            if (!(MyInvocation.BoundParameters.ContainsKey("Key")))
            {
                key = null;
            }

            if (MyInvocation.BoundParameters.ContainsKey("Value"))
            {
                WriteObject(ValueKey.Get(path, key, val), true);
            }
            else
            {
                foreach (ValueKey vk in ValueKey.GetInstances(path, key))
                {
                    WriteObject(vk, true);
                }
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetRegistryValueCommand
}