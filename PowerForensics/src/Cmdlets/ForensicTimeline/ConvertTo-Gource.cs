using System;
using System.Management.Automation;
using PowerForensics.Formats;

namespace PowerForensics.Cmdlets
{
    #region ConvertToGourceCommand
    
    /// <summary> 
    /// This class implements the ConvertTo-Gource cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsData.ConvertTo, "Gource")]
    public class ConvertToGourceCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public ForensicTimeline[] InputObject
        {
            get { return inputobject; }
            set { inputobject = value; }
        }
        private ForensicTimeline[] inputobject;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// 
        /// </summary> 
        protected override void ProcessRecord()
        {
            WriteObject(PowerForensics.Formats.Gource.GetInstances(inputobject), true);
        }

        #endregion Cmdlet Overrides
    }

    #endregion ConvertToForensicTimelineCommand
}
