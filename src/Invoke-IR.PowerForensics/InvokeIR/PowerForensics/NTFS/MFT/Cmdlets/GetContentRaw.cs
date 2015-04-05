using System;
using System.Text;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetContentRawCommand
    /// <summary> 
    /// This class implements the Get-ContentRaw cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "ContentRaw", SupportsShouldProcess = true)]
    public class GetContentRawCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// raw bytes that will be returned.
        /// </summary> 

        [Alias("FilePath")]
        [Parameter(Mandatory = true, ParameterSetName = "Path", Position = 0)]
        public string Path
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

        [Parameter()]
        public Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }
        private Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding encoding;

        [Alias("First", "Head")]
        [Parameter()]
        public Int64 TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
        private Int64 totalCount;

        [Alias("Last")]
        [Parameter()]
        public Int64 Tail
        {
            get { return tail; }
            set { tail = value; }
        }
        private Int64 tail;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            int indexNo = 0;
            byte[] contentArray = null;

            #region Encoding

            System.Text.Encoding contentEncoding = System.Text.Encoding.Default;
            bool asBytes = false;

            if(this.MyInvocation.BoundParameters.ContainsKey("Encoding"))
            {
                if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.Ascii)
                {
                    contentEncoding = System.Text.Encoding.ASCII;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.BigEndianUnicode)
                {
                    contentEncoding = System.Text.Encoding.BigEndianUnicode;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.Byte)
                {
                    asBytes = true;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.String)
                {
                    contentEncoding = System.Text.Encoding.Unicode;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.Unicode)
                {
                    contentEncoding = System.Text.Encoding.Unicode;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.Unknown)
                {
                    asBytes = true;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.UTF7)
                {
                    contentEncoding = System.Text.Encoding.UTF7;
                }
                else if (encoding == Microsoft.PowerShell.Commands.FileSystemCmdletProviderEncoding.UTF8)
                {
                    contentEncoding = System.Text.Encoding.UTF8;
                }
            }

            #endregion Encoding

            if (this.MyInvocation.BoundParameters.ContainsKey("Path"))
            {
                string volLetter = filePath.Split('\\')[0];
                string volume = @"\\.\" + volLetter;
                indexNo = NTFS.IndexNumber.Get(volume, filePath);
                contentArray = MFTRecord.getFile(volume, indexNo);
            }

            else if(this.MyInvocation.BoundParameters.ContainsKey("IndexNumber"))
            {
                Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");
                if (lettersOnly.IsMatch(volume))
                {
                    volume = @"\\.\" + volume + ":";
                }
                indexNo = index;
                contentArray = MFTRecord.getFile(volume, indexNo);
            }

            if (asBytes)
            {
                WriteObject(contentArray);
            }
            else
            {
                string[] outputArray = contentEncoding.GetString(contentArray).Split('\n');

                if (this.MyInvocation.BoundParameters.ContainsKey("TotalCount") && this.MyInvocation.BoundParameters.ContainsKey("Tail"))
                {
                    throw new InvalidOperationException("The parameters TotalCount and Tail cannot be used together. Please specify only one parameter.");
                }
                else if (this.MyInvocation.BoundParameters.ContainsKey("TotalCount"))
                {
                    for (int i = 0; (i < totalCount) && (i < outputArray.Length); i++)
                    {
                        WriteObject(outputArray[i]);
                    }
                }
                else if (this.MyInvocation.BoundParameters.ContainsKey("Tail"))
                {
                    for (long i = tail; (i > 0); i--)
                    {
                        if (i > outputArray.Length)
                        {
                            i = outputArray.Length;
                        }

                        WriteObject(outputArray[outputArray.Length - i]);
                    }
                }
                else
                {
                    WriteObject(outputArray);
                }
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetContentRawCommand class. 

    #endregion GetContentRawCommand

}
