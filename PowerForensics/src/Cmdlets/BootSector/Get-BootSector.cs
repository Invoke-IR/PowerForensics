using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetBootSectorCommand

    /// <summary> 
    /// This class implements the Get-BootSector cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicBootSector", SupportsShouldProcess = true)]
    public class GetBootSectorCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Path of the Drive  
        /// for the BootSector that will be returned.
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
        /// This parameter causes Get-BootSector to return the MBR or GPT as a Byte array
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
        /// The ProcessRecord outputs a MasterBootRecord GuidPartitionTable object for the specified Device
        /// </summary> 
        protected override void ProcessRecord()
        {
            MasterBootRecord mbr = MasterBootRecord.Get(drivePath);

            if (mbr.PartitionTable[0].SystemId == "EFI_GPT_DISK")
            {
                if (asBytes)
                {
                    WriteObject(GuidPartitionTable.GetBytes(drivePath));
                }
                else
                {
                    WriteObject(GuidPartitionTable.Get(drivePath));
                }
            }
            else
            {
                if (asBytes)
                {
                    WriteObject(MasterBootRecord.GetBytes(drivePath));
                }
                else
                {
                    WriteObject(mbr);
                }
            }
        } 

        #endregion Cmdlet Overrides
    }

    #endregion GetBootSectorCommand
}
