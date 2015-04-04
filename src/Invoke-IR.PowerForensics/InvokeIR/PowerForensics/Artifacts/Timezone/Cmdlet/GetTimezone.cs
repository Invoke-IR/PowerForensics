using System;
using System.Management.Automation;

namespace InvokeIR.PowerForensics.Artifacts
{

    #region GetTimeZoneCommand
    /// <summary> 
    /// This class implements the Get-Timezone cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "TimeZone")]
    public class GetTimeZoneCommand : PSCmdlet
    {

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls TimeZone.CurrentTimeZone to return a TimeZone object.
        /// </summary> 
        protected override void ProcessRecord()
        {

            // Output the current TimeZone object
            WriteObject(TimeZone.CurrentTimeZone);
            

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetTimeZoneCommand class. 
    
    #endregion GetTimeZoneCommand

}
