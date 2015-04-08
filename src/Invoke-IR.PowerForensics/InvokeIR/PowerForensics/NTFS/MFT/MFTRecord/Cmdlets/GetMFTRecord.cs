using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlet
{

    #region GetMFTRecordCommand
    /// <summary> 
    /// This class implements the Get-MFTRecord cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "MFTRecord", DefaultParameterSetName = "Zero", SupportsShouldProcess = true)]
    public class GetMFTRecordCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Name of the Volume
        /// for which the FileRecord object should be
        /// returned.
        /// </summary> 

        [Parameter(Mandatory = true, Position = 0)]
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

        [Alias("IndexNumber")]
        [Parameter(Mandatory = true, ParameterSetName = "Index")]
        public int Index
        {
            get { return indexNumber; }
            set { indexNumber = value; }
        }
        private int indexNumber;

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

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 

        [Parameter(Mandatory = false, ParameterSetName = "Path")]
        [Parameter(Mandatory = false, ParameterSetName = "Index")]
        public SwitchParameter AsBytes
        {
            get { return asbytes; }
            set { asbytes = value; }
        }
        private SwitchParameter asbytes;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord instantiates a NTFSVolumeData objects that
        /// corresponds to the VolumeName that is specified.
        /// </summary> 

        protected override void ProcessRecord()
        {

            NativeMethods.getVolumeName(ref volume);

            string volLetter = volume.TrimStart('\\').TrimStart('.').TrimStart('\\') + '\\';

            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            if(this.MyInvocation.BoundParameters.ContainsKey("Path"))
            {
                
                int index = IndexNumber.Get(volume, filePath);
                
                if (asbytes)
                {
                    WriteObject(MFTRecord.getMFTRecordBytes(mftBytes, index));
                }

                else
                {
                    WriteObject(MFTRecord.Get(mftBytes, index, volLetter, filePath));
                }
            }

            else if(this.MyInvocation.BoundParameters.ContainsKey("Index"))
            {
                if (asbytes)
                {
                    WriteObject(MFTRecord.getMFTRecordBytes(mftBytes, indexNumber));
                }

                else
                {
                    WriteObject(MFTRecord.Get(mftBytes, indexNumber, volLetter, null));
                }
            }

            else
            {
                MFTRecord[] records = MFTRecord.GetInstances(mftBytes, volLetter);
                
                foreach(MFTRecord record in records)
                {
                    WriteObject(record);
                }
            }
            

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetFSstatCommand class. 

    #endregion GetMFTRecordCommand

}
