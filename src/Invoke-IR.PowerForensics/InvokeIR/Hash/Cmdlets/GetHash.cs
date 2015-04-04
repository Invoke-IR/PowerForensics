using System;
using System.Management.Automation;
using System.Security.Cryptography;

namespace InvokeIR
{

    #region GetHashCommand
    /// <summary> 
    /// This class implements the Get-Hash cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "Hash")]
    public class GetHashCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the path for the file
        /// that will be hashed
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        /// <summary> 
        /// This parameter provides the count of bytes
        /// that should be included when submitted to be hashed.
        /// </summary> 

        [Parameter()]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private int count;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls the ReadAllBytes method to read the
        /// file into a byte array, which is then passed to the MD5Hash.Get
        /// method to calulate the MD5 hash for the file.
        /// </summary> 
        protected override void ProcessRecord()
        {

            // Read filePath into byte array
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            
            // If the Size parameter is not used the set count to full size of bytes
            if(!(this.MyInvocation.BoundParameters.ContainsKey("Size")))
            {
                count = bytes.Length;
            }

            //Output the computed MD5 Hash as a string to the PowerShell pipeline
            WriteObject(MD5Hash.Get(bytes, count));
            
        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetHashCommand class. 

    #endregion GetHashCommand

}
