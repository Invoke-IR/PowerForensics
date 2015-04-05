using System;
using System.IO;
using System.Management.Automation;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetMBRCommand
    /// <summary> 
    /// This class implements the Get-PartitionTable cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "MBR", SupportsShouldProcess = true)]
    public class GetMBRCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Path of the Drive  
        /// for the MBR that will be returned.
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
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            IntPtr hDrive = NativeMethods.getHandle(drivePath);
            FileStream streamToRead = NativeMethods.getFileStream(hDrive);

            MasterBootRecord mbr = new MasterBootRecord(streamToRead);

            //WriteObject(MasterBootRecord.MBRCodeArea);
            WriteObject(mbr);

            streamToRead.Close();

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetMBRCommand class. 

    #endregion GetMBRCommand


}
