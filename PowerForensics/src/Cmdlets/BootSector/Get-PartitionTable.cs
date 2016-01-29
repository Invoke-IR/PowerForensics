using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetPartitionTableCommand

    /// <summary> 
    /// This class implements the Get-PartitionTable cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicPartitionTable")]
    public class GetPartitionTableCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the DriveName for the 
        /// Partition Table that will be returned.
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

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a MasterBootRecord Object
        /// and outputs all Partitions that are not of the EMPTY type
        /// </summary> 
        protected override void ProcessRecord()
        {
            MasterBootRecord mbr = MasterBootRecord.Get(drivePath);

            if (mbr.PartitionTable[0].SystemId != "EFI_GPT_DISK")
            {
                WriteObject(mbr.GetPartitionTable(), true);
            }
            else
            {
                WriteObject(GuidPartitionTable.Get(drivePath).GetPartitionTable(), true);
            }

        }

        #endregion Cmdlet Overrides
    }

    #endregion GetPartitionTableCommand
}
