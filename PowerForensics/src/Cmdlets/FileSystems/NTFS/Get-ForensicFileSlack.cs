using System.Management.Automation;
using PowerForensics.Ntfs;


namespace PowerForensics.Cmdlets
{
    #region GetFileSlackCommand

    /// <summary> 
    /// This class implements the Get-MftSlack cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicFileSlack", DefaultParameterSetName = "ByIndex")]
    public class GetFileSlackCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        /// 
        /// returned.
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
        /// 
        /// </summary> 
        [Parameter(Position = 0, ParameterSetName = "ByIndex")]
        public int Index
        {
            get { return indexNumber; }
            set { indexNumber = value; }
        }
        private int indexNumber;

        /// <summary>
        /// 
        /// </summary> 
        [Alias("FullName")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ByPath", ValueFromPipelineByPropertyName = true)]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        private string path;

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
        /// The ProcessRecord instantiates a FileRecord objects that
        /// corresponds to the file(s) that is/are specified.
        /// </summary> 
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByIndex":
                    if (MyInvocation.BoundParameters.ContainsKey("Index"))
                    {
                        WriteObject(FileRecord.Get(volume, indexNumber, true).GetSlack());
                    }
                    else
                    {
                        foreach (FileRecord record in FileRecord.GetInstances(volume))
                        {
                            WriteObject(record.GetSlack());
                        }
                    }
                    break;
                case "ByPath":
                    WriteObject(FileRecord.Get(path, true).GetSlack());
                    break;

            }
        }

        #endregion Cmdlet Overrides
    }

    #endregion GetFileSlackCommand
}

