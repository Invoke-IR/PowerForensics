using System;
using System.IO;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetVolumeBootRecordCommand
    /// <summary> 
    /// This class implements the Get-GetVolumeBootRecord cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "VolumeBootRecord")]
    public class GetVolumeBootRecordCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the VolumeName for the 
        /// Volume Boot Record that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, Position = 0)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// This parameter provides causes Get-VolumeBootRecord
        /// to return the VBR as a byte array.
        /// </summary> 

        [Parameter()]
        public SwitchParameter AsBytes
        {
            get { return asbytes; }
            set { asbytes = value; }
        }
        private SwitchParameter asbytes;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method instantiates a VolumeBootRecord object based 
        /// on the volume name given as an argument.
        /// </summary> 
        protected override void ProcessRecord()
        {

            if (asbytes)
            {
                WriteObject(VolumeBootRecord.getBytes(volume));
            }
            else
            {
                WriteObject(new VolumeBootRecord(volume));
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetVolumeBootRecordCommand class. 

    #endregion GetVolumeBootRecordCommand

}
