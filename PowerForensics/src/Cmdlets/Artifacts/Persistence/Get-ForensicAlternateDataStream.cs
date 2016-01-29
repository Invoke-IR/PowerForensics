using System.Management.Automation;
using PowerForensics.Ntfs;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetAlternateDataStreamCommand

    /// <summary> 
    /// This class implements the Get-AlternateDataStream cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicAlternateDataStream", DefaultParameterSetName = "ByVolume")]
    public class GetAlternateDataStreamCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
        [ValidatePattern(@"^(\\\\\.\\)?[A-Zaz]:$")]
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
        [Alias("FullName")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath")]
        public string Path
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a FileRecord objects that
        /// corresponds to the file(s) that is/are specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(AlternateDataStream.GetInstances(volume), true);
                    break;
                case "ByPath":
                    WriteObject(AlternateDataStream.GetInstancesByPath(filePath), true);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    } 

    #endregion GetAlternateDataStreamCommand
}
