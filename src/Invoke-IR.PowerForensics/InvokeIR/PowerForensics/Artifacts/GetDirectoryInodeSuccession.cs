using System;
using System.Management.Automation;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics
{
    #region GetDirectoryIndexSuccessionCommand
    /// <summary> 
    /// This class implements the Get-DirectoryIndexSuccession cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "DirectoryIndexSuccession", SupportsShouldProcess = true)]
    public class GetDirectoryIndexSuccessionCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the DriveName for the 
        /// Partition Table that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }
        private string directory;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            string volume = @"\\.\" + directory.Split('\\')[0];
            string volLetter = directory.Split('\\')[0] + '\\';

            byte[] mftBytes = MasterFileTable.GetBytes(volume);

            string[] files = System.IO.Directory.GetFiles(directory);
            foreach(string file in files)
            {
                WriteObject(MFTRecord.Get(mftBytes, IndexNumber.Get(volume, file), volLetter, file));
            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetDirectoryIndexSuccessionCommand class. 

    #endregion GetDirectoryIndexSuccessionCommand

}
