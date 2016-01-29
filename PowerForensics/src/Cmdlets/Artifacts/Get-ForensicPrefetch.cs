using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetPrefetchCommand

    /// <summary> 
    /// This class implements the Get-Prefetch cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicPrefetch", DefaultParameterSetName = "ByVolume")]
    public class GetPrefetchCommand : PSCmdlet
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
        /// This parameter provides the the path of the Prefetch file to parse.
        /// </summary> 
        [Alias("FullName")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath", ValueFromPipelineByPropertyName = true)]
        public string Path
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        /// <summary> 
        ///
        /// </summary> 
        [Parameter()]
        public SwitchParameter Fast
        {
            get { return fast; }
            set { fast = value; }
        }
        private SwitchParameter fast;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method returns a Prefetch object for the File specified
        /// by the Path property, or iterates through all .pf files in the
        /// C:\Windows\Prefetch directory to output an array of Prefetch objects.
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    if (fast)
                    {
                        WriteObject(Prefetch.GetInstances(volume, fast), true);
                    }
                    else
                    {
                        WriteObject(Prefetch.GetInstances(volume), true);
                    }
                    break;
                case "ByPath":
                    if (fast)
                    {
                        // Output the Prefetch object for the corresponding file
                        WriteObject(Prefetch.Get(filePath, fast), true);
                    }
                    else
                    {
                        // Output the Prefetch object for the corresponding file
                        WriteObject(Prefetch.Get(filePath), true);
                    }
                    break;
            }
        }  

        #endregion Cmdlet Overrides
    }
 
    #endregion GetPrefetchCommand
}
