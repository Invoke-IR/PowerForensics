using System;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.PowerForensics.NTFS.MFT;

namespace InvokeIR.PowerForensics.NTFS.MFT.Attributes
{

    #region GetAttributeCommand
    /// <summary> 
    /// This class implements the Get-Attribute cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "Attribute")]
    public class GetAttributeCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
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

        [Parameter(Mandatory = true)]
        public int IndexNumber
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

            byte[] recordBytes = MFT.MFTRecord.getMFTRecordBytes(volume, indexNumber);
            WriteObject(Attr.Get(recordBytes, attribute));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetProcCommand class. 
    #endregion GetAttributeCommand

}
