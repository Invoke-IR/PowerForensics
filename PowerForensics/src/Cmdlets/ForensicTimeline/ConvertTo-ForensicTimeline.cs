using System;
using System.Management.Automation;
using PowerForensics.Artifacts;
using PowerForensics.Formats;
using PowerForensics.Ntfs;
using PowerForensics.EventLog;
using PowerForensics.Registry;

namespace PowerForensics.Cmdlets
{
    #region ConvertToForensicTimelineCommand
    
    /// <summary> 
    /// This class implements the ConvertTo-ForensicTimeline cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsData.ConvertTo, "ForensicTimeline")]
    public class ConvertToForensicTimelineCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the MFTRecord object(s) to
        /// derive Mactime objects from.
        /// </summary> 
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public PSObject InputObject
        {
            get { return inputobject; }
            set { inputobject = value; }
        }
        private PSObject inputobject;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// 
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (inputobject.TypeNames[0])
            {
                case "PowerForensics.Artifacts.Amcache":
                    break;
                case "PowerForensics.Artifacts.Prefetch":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as Prefetch), true);
                    break;
                case "PowerForensics.Artifacts.ScheduledJob":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as ScheduledJob), true);
                    break;
                case "PowerForensics.Artifacts.ShellLink":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as ShellLink), true);
                    break;
                case "PowerForensics.Artifacts.UserAssist":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as UserAssist), true);
                    break;
                case "PowerForensics.EventLog.EventRecord":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as EventRecord), true);
                    break;
                case "PowerForensics.Ntfs.FileRecord":
                    FileRecord r = inputobject.BaseObject as FileRecord;
                    try
                    {
                        WriteObject(ForensicTimeline.Get(r), true);
                    }
                    catch
                    {
                        
                    }
                    break;
                case "PowerForensics.Ntfs.UsnJrnl":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as UsnJrnl), true);
                    break;
                case "PowerForensics.Registry.NamedKey":
                    WriteObject(ForensicTimeline.Get(inputobject.BaseObject as NamedKey), true);
                    break;
                default:
                    throw new Exception(String.Format("{0} type not supported by ConvertTo-ForensicTimeline", inputobject.TypeNames[0]));
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion ConvertToForensicTimelineCommand
}
