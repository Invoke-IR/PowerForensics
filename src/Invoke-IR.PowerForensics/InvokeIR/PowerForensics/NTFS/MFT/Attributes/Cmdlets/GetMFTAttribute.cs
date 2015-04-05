using System;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetMFTAttributeCommand
    /// <summary> 
    /// This class implements the Get-MFTAttribute cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "MFTAttribute")]
    public class GetMFTAttributeCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true, Position = 0)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// This parameter provides the MFTIndexNumber for the 
        /// FileRecord object that will be returned.
        /// </summary> 

        [Alias("IndexNumber")]
        [Parameter(Mandatory = true)]
        public int Index
        {
            get { return indexNumber; }
            set { indexNumber = value; }
        }
        private int indexNumber;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public uint AttributeId
        {
            get { return attribute; }
            set { attribute = value; }
        }
        private uint attribute;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
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
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";

            }

            WriteDebug("VolumeName: " + volume);

            byte[] recordBytes = MFTRecord.getMFTRecordBytes(volume, indexNumber);
            
            if(asbytes)
            {
                WriteObject(Attr.GetBytes(recordBytes, attribute));
            }

            else
            {
                WriteObject(Attr.Get(recordBytes, attribute));
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetProcCommand class. 
    #endregion GetMFTAttributeCommand

}
