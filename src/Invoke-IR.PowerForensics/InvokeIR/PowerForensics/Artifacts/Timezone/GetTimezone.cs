using System;
using System.Management.Automation;

namespace InvokeIR.PowerForensics.Artifacts
{

    #region GetTimeZoneCommand
    /// <summary> 
    /// This class implements the Get-Prefetch cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "TimeZone")]
    public class GetTimeZoneCommand : PSCmdlet
    {
        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {

            WriteObject(TimeZone.CurrentTimeZone);
            

        } // ProcessRecord 

        #endregion Cmdlet Overrides
    } // End GetProcCommand class. 
    #endregion GetTimeZoneCommand

}
