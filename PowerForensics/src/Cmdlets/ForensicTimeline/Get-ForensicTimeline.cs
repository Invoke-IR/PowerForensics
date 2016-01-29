using System.Management.Automation;
using PowerForensics;
using PowerForensics.Artifacts;
using PowerForensics.EventLog;
using PowerForensics.Formats;
using PowerForensics.Ntfs;
using PowerForensics.Registry;

namespace PowerForensics.Cmdlets
{
    #region GetForensicTimelineCommand

    /// <summary> 
    /// This class implements the Get-ForensicTimeline cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicTimeline")]
    public class GetForensicTimelineCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Parameter(Position = 0)]
        [ValidatePattern(@"^(\\\\\.\\)?[A-Zaz]:$")]
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
            Helper.getVolumeName(ref volume);
            volLetter = Helper.GetVolumeLetter(volume);
        }

        /// <summary> 
        ///
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                //WriteVerbose("Getting Prefetch Instances");
                //WriteObject(ForensicTimeline.GetInstances(Prefetch.GetInstances(volume)), true);
            }
            catch
            {
                //WriteWarning("Error getting Prefetch Instances");
            }

            try
            {
                WriteVerbose("Getting ScheduledJob Instances");
                WriteObject(ForensicTimeline.GetInstances(ScheduledJob.GetInstances(volume)), true);
            }
            catch
            {
                WriteWarning("Error getting ScheduledJob Instances");
            }

            try
            {
                WriteVerbose("Getting ShellLink Instances");
                WriteObject(ForensicTimeline.GetInstances(ShellLink.GetInstances(volume)), true);
            }
            catch
            {
                WriteWarning("Error getting ShellLink Instances");
            }

            try
            {
                WriteVerbose("Getting FileRecord Instances");
                WriteObject(ForensicTimeline.GetInstances(FileRecord.GetInstances(volume)), true);
            }
            catch
            {
                WriteWarning("Error getting FileRecord Instances");
            }

            try
            {
                WriteVerbose("Getting UsnJrnl Instances");
                WriteObject(ForensicTimeline.GetInstances(UsnJrnl.GetInstances(volume)), true);
            }
            catch
            {
                WriteWarning("Error getting UsnJrnl Instances");
            }

            try
            {
                //WriteVerbose("Getting EventLog Instances");
                //WriteObject(ForensicTimeline.GetInstances(EventRecord.GetInstances(volume)), true);
            }
            catch
            {
                //WriteWarning("Error getting EventLog Instances");
            }

            try
            {
                WriteVerbose("Getting DRIVERS Hive Keys");
                WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\DRIVERS")), true);
            }
            catch
            {
                WriteWarning("Error getting DRIVERS Hive Keys");
            }

            try
            {
                WriteVerbose("Getting SAM Hive Keys");
                WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SAM")), true);
            }
            catch
            {
                WriteWarning("Error getting SAM Hive Keys");
            }

            try
            {
                WriteVerbose("Getting SECURITY Hive Keys");
                WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SECURITY")), true);
            }
            catch
            {
                WriteWarning("Error getting SECURITY Hive Keys");
            }

            try
            {
                WriteVerbose("Getting SOFTWARE Hive Keys");
                WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SOFTWARE")), true);
            }

            catch
            {
                WriteWarning("Error getting SOFTWARE Hive Keys");
            }
            
            try
            {
                WriteVerbose("Getting SYSTEM Hive Keys");
                WriteObject(ForensicTimeline.GetInstances(NamedKey.GetInstancesRecurse(volLetter + "\\Windows\\system32\\config\\SYSTEM")), true);
            }
            catch
            {
                WriteWarning("Error getting SYSTEM Hive Keys");
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetForensicTimelineCommand
}
