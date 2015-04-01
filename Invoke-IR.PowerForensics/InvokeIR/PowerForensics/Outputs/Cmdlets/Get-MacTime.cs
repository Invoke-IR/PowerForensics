using System;
using System.Management.Automation;
using InvokeIR.PowerForensics.NTFS.MFT;

namespace InvokeIR.PowerForensics
{

    #region GetMacTimeCommand
    /// <summary> 
    /// This class implements the Get-MacTime cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "MacTime")]
    public class GetMacTimeCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the DriveName for the 
        /// Partition Table that will be returned.
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
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {

            foreach(MFTRecord record in mftRecord)
            {
                WriteObject(mactime.Get(record));
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetMacTimeCommand class. 

    #endregion GetMacTimeCommand

}
