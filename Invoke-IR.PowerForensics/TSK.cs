using System;
using System.Text.RegularExpressions;
using System.Management.Automation;

namespace InvokeIR.PowerForensics.TSK
{

    #region GetFSStatCommand
    /// <summary> 
    /// This class implements the Get-FSStat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "FSStat", SupportsShouldProcess = true)]
    public class GetFSStatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the NTFSVolumeData object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string VolumeName
        {
            get { return volumeName; }
            set { volumeName = value; }
        }
        private string volumeName;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a NTFSVolumeData objects that
        /// corresponds to the VolumeName that is specified.
        /// </summary> 

        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volumeName))
            {

                volumeName = @"\\.\" + volumeName + ":";

            }

            WriteDebug("VolumeName: " + volumeName);

            NTFSVolumeData volumeData = InvokeIR.PowerForensics.Main.getVolumeDataInformation(volumeName);
            WriteObject(volumeData);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetFSstatCommand

    #region GetIStatCommand
    /// <summary> 
    /// This class implements the Get-IStat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "IStat", DefaultParameterSetName = "One", SupportsShouldProcess = true)]
    public class GetIStatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
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

        [Parameter(Mandatory = true, ParameterSetName = "One")]
        public uint IndexNumber
        {
            get { return indexNumber; }
            set { indexNumber = value; }
        }
        private uint indexNumber;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
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
        /// The ProcessRecord instantiates a NTFSVolumeData objects that
        /// corresponds to the VolumeName that is specified.
        /// </summary> 

        protected override void ProcessRecord()
        {

            if (this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {

                indexNumber = InvokeIR.PowerForensics.Main.getInode(filePath);

            }

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";

            }

            WriteDebug("VolumeName: " + volume);

            FileRecord MFTRecord = InvokeIR.PowerForensics.Main.getFileRecord(volume, indexNumber);
            WriteObject(MFTRecord);

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetIStatCommand

    #region GetICatCommand
    /// <summary> 
    /// This class implements the Get-ICat cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "ICat", SupportsShouldProcess = true)]
    public class GetICatCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// raw bytes that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        private string filePath;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord outputs the raw bytes of the specified File
        /// </summary> 

        protected override void ProcessRecord()
        {

            WriteObject(InvokeIR.PowerForensics.Main.getFile(filePath));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetIStatCommand

}
