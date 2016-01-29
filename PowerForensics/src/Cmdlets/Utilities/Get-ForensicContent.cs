using System;
using System.Management.Automation;
using PowerForensics.Ntfs;

namespace PowerForensics.Cmdlets
{
    #region GetContentCommand

    /// <summary> 
    /// This class implements the Get-Content cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicContent", DefaultParameterSetName = "ByPath")]
    public class GetContentRawCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// raw bytes that will be returned.
        /// </summary> 

        [Alias("FullName")]
        [Parameter(Mandatory = true, Position = 0 , ParameterSetName = "ByPath")]
        public string Path
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        [Parameter(ParameterSetName = "ByIndex")]
        [ValidatePattern(@"^(\\\\\.\\)?[A-Zaz]:$")]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        [Parameter(Mandatory = true, ParameterSetName = "ByIndex")]
        public int Index
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
            byte[] contentArray = null;

            #region Encoding

            System.Text.Encoding contentEncoding = System.Text.Encoding.ASCII;
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
                contentArray = FileRecord.Get(filePath, true).GetContent();
            }

            else if(this.MyInvocation.BoundParameters.ContainsKey("Index"))
            {
                Helper.getVolumeName(ref volume);
                contentArray = FileRecord.Get(volume, index, true).GetContent();
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

        }

        #endregion Cmdlet Overrides
    }

    #endregion GetContentCommand
}
