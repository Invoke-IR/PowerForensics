using System;
using System.Management.Automation;
using InvokeIR.PowerForensics.Formats;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetMactimeCommand
    /// <summary> 
    /// This class implements the Get-Mactime cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "Mactime")]
    public class GetMactimeCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the MFTRecord object(s) to
        /// derive Mactime objects from.
        /// </summary> 

        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public MFTRecord[] MFTRecord
        {
            get { return mftRecord; }
            set { mftRecord = value; }
        }
        private MFTRecord[] mftRecord;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls Mactime.Get() 
        /// method to return an array of Mactime objects
        /// for the inputted MFTRecord object.
        /// </summary> 
        protected override void ProcessRecord()
        {

            // Iterate through each MFTRecord provided as input
            foreach(MFTRecord record in mftRecord)
            {
                // Create an array of Mactime objects for the current MFTRecord object
                WriteObject(Mactime.Get(record));
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetMactimeCommand class. 

    #endregion GetMactimeCommand

}
