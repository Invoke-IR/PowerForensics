using System.Management.Automation;
using PowerForensics.Ntfs;

namespace PowerForensics.Cmdlets
{
    #region CopyFileCommand

    /// <summary> 
    /// This class implements the Copy-File cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Copy, "ForensicFile")]
    public class CopyFileCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Alias("FullName")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByPath")]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string path;

        /// <summary> 
        /// This parameter provides the MFTIndexNumber for the 
        /// FileRecord object that will be returned.
        /// </summary> 
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByIndex")]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        private int index;

        /// <summary> 
        /// 
        /// </summary> 
        [Parameter(ParameterSetName = "ByIndex")]
        [ValidatePattern(@"^(\\\\\.\\)?[A-Zaz]:$")]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        /// <summary> 
        /// This parameter provides the MFTIndexNumber for the 
        /// FileRecord object that will be returned.
        /// </summary> 
        [Parameter(Mandatory = true, Position = 1)]
        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }
        private string destination;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// 
        /// </summary> 
        protected override void BeginProcessing()
        {
            if (ParameterSetName == "ByIndex")
            {
                Helper.getVolumeName(ref volume);
            }
        }

        /// <summary> 
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByPath":
                    FileRecord record = FileRecord.Get(path, true);
                    record.CopyFile(destination);
                    break;
                case "ByVolume":
                    FileRecord rec = FileRecord.Get(volume, index, true);
                    rec.CopyFile(destination);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }
    #endregion CopyFileCommand
}
