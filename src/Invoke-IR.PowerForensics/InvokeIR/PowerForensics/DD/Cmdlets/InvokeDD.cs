using System;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.PowerForensics;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region InvokeDDCommand
    /// <summary> 
    /// This class implements the Invoke-DD cmdlet. 
    /// </summary> 

    [Cmdlet("Invoke", "DD")]
    public class InvokeDDCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the File or Volume
        /// that should be read from (Ex. \\.\C: or C).
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string InFile
        {
            get { return inFile; }
            set { inFile = value; }
        }
        private string inFile;

        /// <summary> 
        /// This parameter provides the Name of the File
        /// that the read bytes should be output to.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string OutFile
        {
            get { return outFile; }
            set { outFile = value; }
        }
        private string outFile;

        /// <summary> 
        /// This parameter provides the Offset into the 
        /// specified InFile to begin reading.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public ulong Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private ulong offset;

        /// <summary> 
        /// This parameter provides the size of blocks to be
        /// read from the specified InFile.
        /// </summary> 

        [Parameter()]
        public uint BlockSize
        {
            get { return blockSize; }
            set { blockSize = value; }
        }
        private uint blockSize;

        /// <summary> 
        /// This parameter provides the Count of Blocks
        /// to read from the specified InFile.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public uint Count
        {
            get { return count; }
            set { count = value; }
        }
        private uint count;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord Reads bytes from the InFile
        /// and outputs them to the OutFile.
        /// </summary> 

        protected override void ProcessRecord()
        {

            if(!(this.MyInvocation.BoundParameters.ContainsKey("BlockSize")))
            {
                blockSize = 512;
            }

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(inFile))
            {

                inFile = @"\\.\" + inFile + ":";

            }

            WriteDebug("VolumeName: " + inFile);

            DD.Get(inFile, outFile, offset, blockSize, count);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End InvokeDDCommand class. 

    #endregion InvokeDDCommand

}
