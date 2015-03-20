using System;
using System.Management.Automation;

namespace InvokeIR.PowerForensics.NTFS.MFT
{

    #region GetICatCommand
    /// <summary> 
    /// This class implements the Get-ICat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "RawContent", SupportsShouldProcess = true)]
    public class GetRawContentCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// raw bytes that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            string volLetter = filePath.Split('\\')[0];
            string volume = "\\\\.\\" + volLetter;
            WriteObject(MFTRecord.getFile(volume, filePath));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetIStatCommand

}
