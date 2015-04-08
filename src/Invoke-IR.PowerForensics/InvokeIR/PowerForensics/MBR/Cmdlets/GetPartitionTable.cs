using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Management.Automation;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetPartitionTableCommand
        /// <summary> 
        /// This class implements the Get-PartitionTable cmdlet. 
        /// </summary> 

        [Cmdlet(VerbsCommon.Get, "PartitionTable", SupportsShouldProcess = true)]
        public class GetPartitionTableCommand : PSCmdlet
        {

            #region Parameters

            /// <summary> 
            /// This parameter provides the DriveName for the 
            /// Partition Table that will be returned.
            /// </summary> 

            [Alias("DrivePath")]
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

                MasterBootRecord mbr = new MasterBootRecord(drivePath);

                foreach (MBR.Partition partition in mbr.Partitions)
                {

                    if (partition.SystemID != "EMPTY")
                    {
                        WriteObject(partition);
                    }

                    else if(partition.SystemID.Contains("EXTENDED"))
                    {
                        // Add code to parse EXTENDED partitions
                    }
                }

            } // ProcessRecord 

            #endregion Cmdlet Overrides

        } // End GetPartitionTableCommand class. 

        #endregion GetPartitionTableCommand

}
