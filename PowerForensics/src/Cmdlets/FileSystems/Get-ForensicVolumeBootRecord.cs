using System.Management.Automation;
using PowerForensics.Ntfs;

namespace PowerForensics.Cmdlets
{
    #region GetVolumeBootRecordCommand

    /// <summary> 
    /// This class implements the Get-GetVolumeBootRecord cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicVolumeBootRecord", DefaultParameterSetName = "ByVolume")]
    public class GetVolumeBootRecordCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the VolumeName for the 
        /// Volume Boot Record that will be returned.
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
        /// This parameter provides the path to the $Boot File or Volume Boot Record
        /// </summary>
        [Alias("FullName")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath")]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string path;

        /// <summary> 
        /// This parameter provides causes Get-VolumeBootRecord
        /// to return the VBR as a byte array.
        /// </summary> 
        [Parameter()]
        public SwitchParameter AsBytes
        {
            get { return asbytes; }
            set { asbytes = value; }
        }
        private SwitchParameter asbytes;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method instantiates a VolumeBootRecord object based 
        /// on the volume name given as an argument.
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    if (asbytes)
                    {
                        WriteObject(VolumeBootRecord.GetBytes(volume));
                    }
                    else
                    {
                        WriteObject(VolumeBootRecord.Get(volume));
                    }
                    break;
                case "ByPath":
                    if (asbytes)
                    {
                        WriteObject(VolumeBootRecord.GetBytesByPath(path));
                    }
                    else
                    {
                        WriteObject(VolumeBootRecord.GetByPath(path));
                    }
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetVolumeBootRecordCommand
}
