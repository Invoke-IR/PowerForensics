using System.Management.Automation;
using PowerForensics;
using PowerForensics.Artifacts;
using PowerForensics.EventLog;
using PowerForensics.Formats;
using PowerForensics.Ntfs;
using PowerForensics.Registry;

namespace PowerForensics.Cmdlets
{
    #region InvokeForensicTimelineCommand

    /// <summary> 
    /// This class implements the Invoke-ForensicTimeline cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsLifecycle.Invoke, "Timeline")]
    public class InvokeForensicTimelineCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Parameter(Position = 0)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        #endregion Parameters

        string volLetter = null;

        #region Cmdlet Overrides

        /// <summary> 
        ///
        /// </summary> 
        protected override void BeginProcessing()
        {
            Util.checkAdmin();
            Util.getVolumeName(ref volume);
            volLetter = Util.GetVolumeLetter(volume);
        }

        /// <summary> 
        ///
        /// </summary>
        protected override void ProcessRecord()
        {
            //WriteObject(ForensicTimeline.GetInstances(Prefetch.GetInstances(volume)), true);
            
            WriteVerbose("Getting ScheduledJob Instances");
            WriteObject(ForensicTimeline.GetInstances(ScheduledJob.GetInstances(volume)), true);
            
            WriteVerbose("Getting ShellLink Instances");
            WriteObject(ForensicTimeline.GetInstances(ShellLink.GetInstances(volume)), true);
            
            WriteVerbose("Getting FileRecord Instances");
            WriteObject(ForensicTimeline.GetInstances(FileRecord.GetInstances(volume)), true);
            
            WriteVerbose("Getting UsnJrnl Instances");
            WriteObject(ForensicTimeline.GetInstances(UsnJrnl.GetInstances(volume)), true);
            
            WriteVerbose("Getting EventRecord Instances");
            WriteObject(ForensicTimeline.GetInstances(EventRecord.GetInstances(volume)) , true);
            
            WriteVerbose("Getting DRIVERS Hive Keys");
            WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\DRIVERS")), true);
            
            WriteVerbose("Getting SAM Hive Keys");
            WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SAM")), true);
            
            WriteVerbose("Getting SECURITY Hive Keys");
            WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SECURITY")), true);
            
            WriteVerbose("Getting SOFTWARE Hive Keys");
            WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SOFTWARE")), true);
            
            WriteVerbose("Getting SYSTEM Hive Keys");
            WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SYSTEM")), true);
        }

        #endregion Cmdlet Overrides
    }

    #endregion InvokeForensicTimelineCommand
}
