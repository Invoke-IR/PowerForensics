using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetMasterBootRecordCommand

    /// <summary> 
    /// This class implements the Get-MasterBootRecord cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicMasterBootRecord")]
    public class GetMBRCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Path of the Drive  
        /// for the MBR that will be returned.
        /// </summary> 
        [Alias("DrivePath")]
        [ValidatePattern(@"^\\\\.\\PHYSICALDRIVE\d*$")]
        [Parameter(Mandatory = true, Position = 0)]
        public string Path
        {
            get { return drivePath; }
            set { drivePath = value; }
        }
        private string drivePath;

        /// <summary> 
        /// This parameter causes Get-MBR to return the MBR as a Byte array
        /// </summary> 
        [Parameter()]
        public SwitchParameter AsBytes
        {
            get { return asBytes; }
            set { asBytes = value; }
        }
        private SwitchParameter asBytes;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs a MasterBootRecord object for the
        /// specified Drive Path
        /// </summary> 
        protected override void ProcessRecord()
        {
            if (asBytes)
            {
                WriteObject(MasterBootRecord.GetBytes(drivePath));
            }
            else
            {
                WriteObject(new MasterBootRecord(MasterBootRecord.GetBytes(drivePath)));
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetMasterBootRecordCommand
}

