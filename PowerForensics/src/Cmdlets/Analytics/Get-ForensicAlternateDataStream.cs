using System.Management.Automation;
using PowerForensics.Ntfs;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetAlternateDataStreamCommand

    /// <summary> 
    /// This class implements the Get-AlternateDataStream cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "AlternateDataStream", DefaultParameterSetName = "ByVolume")]
    public class GetAlternateDataStreamCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByVolume")]
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
        /// 
        /// </summary> 
        protected override void BeginProcessing()
        {
            Util.checkAdmin();
            Util.getVolumeName(ref volume);
        }

        /// <summary> 
        /// The ProcessRecord instantiates a FileRecord objects that
        /// corresponds to the file(s) that is/are specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            if (ParameterSetName == "ByPath")
            {
                FileRecord record = FileRecord.Get(filePath, false);

                if (record.Attribute != null)
                {
                    foreach (Attr attr in record.Attribute)
                    {
                        if (attr.Name == Attr.ATTR_TYPE.DATA)
                        {
                            //if (attr.NameString != "")
                            if(attr.NameString.Length > 0)
                            {
                                WriteObject(new AlternateDataStream(record.FullName, record.Name, attr.NameString));
                            }
                        }
                    }
                }
            }
            else
            {
                FileRecord[] records = FileRecord.GetInstances(volume);

                foreach (FileRecord record in records)
                {
                    if (record.Attribute != null)
                    {
                        foreach (Attr attr in record.Attribute)
                        {
                            if (attr.Name == Attr.ATTR_TYPE.DATA)
                            {
                                //if (attr.NameString != "")
                                if (attr.NameString.Length > 0)
                                {
                                    WriteObject(new AlternateDataStream(record.FullName, record.Name, attr.NameString));
                                }
                            }
                        }

                    }
                }
            }
        }

        #endregion Cmdlet Overrides
    } 

    #endregion GetAlternateDataStreamCommand
}
