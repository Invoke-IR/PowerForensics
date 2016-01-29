using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetEventLogCommand

    /// <summary> 
    /// This class implements the Get-EventLog cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicEventLog", DefaultParameterSetName = "ByVolume")]
    public class GetEventLogCommand : PSCmdlet
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
            get { return path; }
            set { path = value; }
        }
        private string path;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a 
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(EventLog.EventRecord.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(EventLog.EventRecord.Get(path), true);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetEventLogCommand
}
