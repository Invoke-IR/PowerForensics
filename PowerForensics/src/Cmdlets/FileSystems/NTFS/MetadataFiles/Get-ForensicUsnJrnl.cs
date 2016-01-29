using System.Management.Automation;
using PowerForensics.Ntfs;

namespace PowerForensics.Cmdlets
{
    #region GetUsnJrnlCommand

    /// <summary> 
    /// This class implements the Get-UsnJrnl cmdlet 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicUsnJrnl", DefaultParameterSetName = "ByVolume")]
    public class GetUsnJrnlCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the the name of the target volume.
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
        [Parameter(Mandatory = true, ParameterSetName = "ByPath", ValueFromPipelineByPropertyName = true)]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string path;

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter()]
        public ulong Usn
        {
            get { return usn; }
            set { usn = value; }
        }
        private ulong usn;

        /// <summary> 
        /// 
        /// </summary> 

        /*[Parameter()]
        public SwitchParameter AsBytes
        {
            get { return asBytes; }
            set { asBytes = value; }
        }
        private SwitchParameter asBytes;*/

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
                    if (MyInvocation.BoundParameters.ContainsKey("Usn"))
                    {
                        WriteObject(UsnJrnl.Get(volume, usn));
                    }
                    else
                    {
                        WriteObject(UsnJrnl.GetInstances(volume), true);
                    }
                    break;
                case "ByPath":
                    if (MyInvocation.BoundParameters.ContainsKey("Usn"))
                    {
                        WriteObject(UsnJrnl.GetByPath(path, usn));
                    }
                    else
                    {
                        WriteObject(UsnJrnl.GetInstancesByPath(path), true);
                    }
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetUsnJrnlCommand
}
