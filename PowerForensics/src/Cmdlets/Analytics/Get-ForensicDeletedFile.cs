using System.Management.Automation;

namespace PowerForensics.Cmdlets
{
    #region GetDeletedFileCommand

    /// <summary> 
    /// This class implements the Get-DeletedFile cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "DeletedFile", DefaultParameterSetName = "Zero", SupportsShouldProcess = true)]
    public class GetDeletedFileCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Parameter(ParameterSetName = "Zero")]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 
        [Alias("FilePath")]
        [Parameter(Mandatory = true, ParameterSetName = "Path")]
        public string Path
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        ///
        /// </summary> 
        protected override void BeginProcessing()
        {
            Util.checkAdmin();
            Util.getVolumeName(ref volume);
        }

        /// <summary> 
        ///
        /// </summary>
        protected override void ProcessRecord()
        {
            /*NativeMethods.getVolumeName(ref volume);
            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            string volLetter = volume.TrimStart('\\').TrimStart('.').TrimStart('\\') + '\\';
            FileRecord[] records = FileRecord.GetInstances(mftBytes, volLetter);

            foreach (FileRecord record in records)
            {
                if (record.Deleted)
                {
                    WriteObject(record);
                }
            }*/
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetDeletedFileCommand
}
