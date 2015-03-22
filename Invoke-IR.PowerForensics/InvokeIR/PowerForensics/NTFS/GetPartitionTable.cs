using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Management.Automation;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.NTFS
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

            [Parameter(Mandatory = true)]
            public string DrivePath
            {
                get { return drivePath; }
                set { drivePath = value; }
            }
            private string drivePath;

            #endregion Parameters

            #region Cmdlet Overrides

            /// <summary> 
            /// The ProcessRecord outputs the raw bytes of the specified File
            /// </summary> 

            protected override void ProcessRecord()
            {

                IntPtr hDrive = NativeMethods.getHandle(drivePath);
                FileStream streamToRead = NativeMethods.getFileStream(hDrive);

                MBR MasterBootRecord = MBR.Get(streamToRead);
                foreach (MBR.MBR_PARTITION_TABLE_ENTRY partition in MasterBootRecord.partitionList)
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

                streamToRead.Close();

            } // ProcessRecord 

            #endregion Cmdlet Overrides

        } // End GetFSstatCommand class. 

        #endregion GetPartitionTableCommand

}
