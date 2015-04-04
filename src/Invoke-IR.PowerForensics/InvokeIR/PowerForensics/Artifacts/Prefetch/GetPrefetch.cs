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

    [Cmdlet(VerbsCommon.Get, "Prefetch", DefaultParameterSetName = "One")]
    public class GetPrefetchCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the list of process names on  
        /// which the Stop-Proc cmdlet will work. 
        /// </summary> 

        [Parameter(Mandatory = true, ParameterSetName = "Two")]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {

            string volLetter = Directory.GetCurrentDirectory().Split('\\')[0];
            string volume = @"\\.\" + volLetter;

            IntPtr hVolume = NativeMethods.getHandle(volume);
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);
            byte[] MFT = MasterFileTable.GetBytes(hVolume, streamToRead);

            if (this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {
                WriteObject(Prefetch.Get(volume, streamToRead, MFT, filePath));
            }

            else
            {

                string prefetchPath = volLetter + @"\\Windows\\Prefetch";
                var pfFiles = System.IO.Directory.GetFiles(prefetchPath, "*.pf");
                foreach (var file in pfFiles)
                {
                    WriteObject(Prefetch.Get(volume, streamToRead, MFT, file));
                }

            }

        } // ProcessRecord 

        #endregion Cmdlet Overrides
    } // End GetProcCommand class. 
    #endregion GetPrefetchCommand

}
