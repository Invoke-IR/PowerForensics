using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetScheduledJobCommand

    /// <summary> 
    /// This class implements the Get-ScheduledJob cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicScheduledJob", DefaultParameterSetName = "ByVolume")]
    public class GetScheduledJobCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        ///
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
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
            Util.checkAdmin();

            if (ParameterSetName == "ByVolume")
            {
                Util.getVolumeName(ref volume);
            }
        }

        /// <summary> 
        /// The ProcessRecord method calls TimeZone.CurrentTimeZone to return a TimeZone object.
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(ScheduledJob.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(ScheduledJob.Get(path));
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetScheduledJobCommand
}
