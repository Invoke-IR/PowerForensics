using System;
using System.IO;
using System.Management.Automation;
using InvokeIR.Win32;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetMBRCommand
    /// <summary> 
    /// This class implements the Get-MBR cmdlet. 
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
        /// The ProcessRecord outputs a MasterBootRecord object for the
        /// specified Drive Path
        /// </summary> 

        protected override void ProcessRecord()
        {

            WriteObject(new MasterBootRecord(drivePath));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetMBRCommand class. 

    #endregion GetMBRCommand


}
