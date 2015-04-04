using System;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace InvokeIR.PowerForensics.NTFS.MFT
{

    #region GetContentByteArrayCommand
    /// <summary> 
    /// This class implements the Get-ContentByteArray cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "ContentByteArray", SupportsShouldProcess = true)]
    public class GetContentByteArrayCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// raw bytes that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, ParameterSetName = "Path")]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        [Parameter(Mandatory = true, ParameterSetName = "Index")]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        [Parameter(Mandatory = true, ParameterSetName = "Index")]
        public int IndexNumber
        {
            get { return index; }
            set { index = value; }
        }
        private int index;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            if(this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {
                string volLetter = filePath.Split('\\')[0];
                string volume = @"\\.\" + volLetter;
                WriteObject(MFTRecord.getFile(volume, filePath));
            }

            else if(this.MyInvocation.BoundParameters.ContainsKey("IndexNumber"))
            {
                Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

                if (lettersOnly.IsMatch(volume))
                {

                    volume = @"\\.\" + volume + ":";

                }

                WriteDebug("VolumeName: " + volume);

                WriteObject(MFTRecord.getFile(volume, index));
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetContentByteArrayCommand

}
