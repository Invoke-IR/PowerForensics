using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetShellLinkCommand

    /// <summary> 
    /// This class implements the Get-ShellLink cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicShellLink", DefaultParameterSetName = "ByVolume")]
    public class GetShellLinkCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        ///
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
        [ValidatePattern(@"^(\\\\\.\\)?[A-Zaz]:$")]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// 
        /// </summary> 
        [Alias("FullName")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath")]
        public string Path
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides
        
        /// <summary> 
        /// 
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(ShellLink.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(ShellLink.Get(filePath));
                    break;
            }
        }  

        #endregion Cmdlet Overrides
    }

    #endregion GetShellLinkCommand
}
