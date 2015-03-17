using System;
using System.Text.RegularExpressions;
using System.Management.Automation;

namespace InvokeIR.PowerForensics.DD
{

/*    public class DD
    {
        public static void dd(string inFile, string outFile, long offset, int blockSize, int count)
        {

            long sizeToRead = blockSize * count;

            // Read sizeToRead bytes from the Volume
            byte[] buffer = Win32.readDrive(inFile, offset, sizeToRead);

            // Open file for reading
            System.IO.FileStream _FileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(buffer, 0, buffer.Length);
            // close file stream
            _FileStream.Close();
        }
    }

    #region ExportPowerDDCommand
    /// <summary> 
    /// This class implements the Export-PowerDD cmdlet. 
    /// </summary> 

    [Cmdlet("Export", "PowerDD", SupportsShouldProcess = true)]
    public class ExportPowerDDCommand : PSCmdlet
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
        public long Offset
        {
            get { return offset; }
            set { offset = value; }
        }
        private long offset;

        /// <summary> 
        /// This parameter provides the size of blocks to be
        /// read from the specified InFile.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public int BlockSize
        {
            get { return blockSize; }
            set { blockSize = value; }
        }
        private int blockSize;

        /// <summary> 
        /// This parameter provides the Count of Blocks
        /// to read from the specified InFile.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private int count;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a Reads bytes from the InFile
        /// and outputs to the OutFile.
        /// </summary> 

        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(inFile))
            {

                inFile = @"\\.\" + inFile + ":";

            }

            WriteDebug("VolumeName: " + inFile);

            InvokeIR.PowerForensics.Main.dd(inFile, outFile, offset, blockSize, count);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End ExportPowerDDCommand class. 

    #endregion ExportPowerDDCommand
    */
}
