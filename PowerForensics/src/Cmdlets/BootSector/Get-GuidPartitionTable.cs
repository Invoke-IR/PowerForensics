using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetGuidPartitioTableCommand

    /// <summary> 
    /// This class implements the Get-GuidPartitioTable cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "GuidPartitionTable", SupportsShouldProcess = true)]
    public class GetGuidPartitioTableCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Path of the Drive  
        /// for the GPT that will be returned.
        /// </summary> 
        [Alias("DrivePath")]
        [Parameter(Mandatory = true, Position = 0)]
        public string Path
        {
            get { return drivePath; }
            set { drivePath = value; }
        }
        private string drivePath;

        /// <summary> 
        /// This parameter causes Get-GPT to return the GPT as a Byte array
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
        /// 
        /// </summary> 
        protected override void BeginProcessing()
        {
            // Ensure cmdlet is being run as Administrator
            Util.checkAdmin();
            // Check that drivePath is valid
            Util.getDriveName(drivePath);
        }

        /// <summary> 
        /// The ProcessRecord outputs a GuidPartitionTable object for the specified Drive Path
        /// </summary> 
        protected override void ProcessRecord()
        {
            if (asBytes)
            {
                WriteObject(GuidPartitionTable.GetBytes(drivePath));
            }
            else
            {
                WriteObject(new GuidPartitionTable(drivePath));
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetGuidPartitioTableCommand
}
