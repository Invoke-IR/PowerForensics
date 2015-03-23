using System;
using System.IO;
using System.Management.Automation;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS.MFT;

namespace InvokeIR.PowerForensics.Artifacts
{

    #region GetPrefetchCommand
    /// <summary> 
    /// This class implements the Get-Prefetch cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "Prefetch", DefaultParameterSetName = "None")]
    public class GetPrefetchCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the the path of the  
        /// Prefetch file to parse.
        /// </summary> 

        [Parameter(Mandatory = true, ParameterSetName = "Path")]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method returns a Prefetch object for the File specified
        /// by the FilePath property, or iterates through all .pf files in the
        /// C:\Windows\Prefetch directory to output an array of Prefetch objects.
        /// </summary> 
        protected override void ProcessRecord()
        {

            // Get current volume
            string volLetter = Directory.GetCurrentDirectory().Split('\\')[0];
            string volume = @"\\.\" + volLetter;

            // Get a handle to the volume
            IntPtr hVolume = NativeMethods.getHandle(volume);
            
            // Create a FileStream to read from the volume handle
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);

            // Get a byte array representing the Master File Table
            byte[] MFT = MasterFileTable.GetBytes(hVolume, streamToRead);

            // If the FilePath parameter is used
            if (this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {
                //Test that FilePath exists
                if(File.Exists(filePath))
                {
                    // Output the Prefetch object for the corresponding file
                    WriteObject(Prefetch.Get(volume, streamToRead, MFT, filePath));
                }
                else
                {
                    throw new FileNotFoundException((filePath + " does not exist.  Please enter a valid file path."));
                }
            }

            else
            {
                // Build Prefetch directory path
                string prefetchPath = volLetter + @"\\Windows\\Prefetch";
                
                // Get list of file in the Prefetch directory that end in the .pf extension
                var pfFiles = System.IO.Directory.GetFiles(prefetchPath, "*.pf");
                
                // Iterate through Prefetch Files
                foreach (var file in pfFiles)
                {
                    // Output the Prefetch object for the corresponding file
                    WriteObject(Prefetch.Get(volume, streamToRead, MFT, file));
                }

            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetProcCommand class.
 
    #endregion GetPrefetchCommand

}
