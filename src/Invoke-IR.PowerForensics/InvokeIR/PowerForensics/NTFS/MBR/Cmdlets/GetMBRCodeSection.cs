using System;
using System.IO;
using System.Management.Automation;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.NTFS.MBR
{

    #region GetMBRCodeSectionCommand
    /// <summary> 
    /// This class implements the Get-PartitionTable cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "MBRCodeSection", SupportsShouldProcess = true)]
    public class GetMBRCodeSectionCommand : PSCmdlet
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

            //WriteObject(MasterBootRecord.MBRCodeArea);
            WriteObject(MasterBootRecord);

            streamToRead.Close();

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetMBRCodeSectionTableCommand


}
