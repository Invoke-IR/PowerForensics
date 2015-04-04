using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text.RegularExpressions;

namespace InvokeIR.PowerForensics.NTFS.MFT
{

    enum SYSTEM_FILES
    {
        MFT = 0,
        MFTMirr = 1,
        LogFile = 2,
        Volume = 3,
        AttrDef = 4,
        Root = 5,
        Bitmap = 6,
        Boot = 7,
        BadClus = 8,
        Secure = 9,
        UpCase = 10,
        Extend = 11,
        Quota = 24,
        ObjId = 25,
        Reparse = 26,
        RmMetadata = 27,
        Repair = 28,
        TxfLog = 29,
        Txf = 30,
        Tops = 31,
        TxfLogblf = 32,
        TxfLogContainer00000000000000000001 = 33,
        TxfLogContainer00000000000000000002 = 34
    }

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

        [Parameter(Mandatory = true, ParameterSetName = "Index")]
        public int IndexNumber
        {
            get { return indexNumber; }
            set { indexNumber = value; }
        }
        private int indexNumber;

        /// <summary> 
        /// This parameter provides the FileName for the 
        /// FileRecord object that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, ParameterSetName = "Path")]
        public string FilePath
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

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";
                
            }

            WriteDebug("VolumeName: " + volume);

            byte[] mftBytes = MasterFileTable.GetBytes(volume);
            
            if(this.MyInvocation.BoundParameters.ContainsKey("FilePath"))
            {
                int index = 0;

                if(Enum.IsDefined(typeof(SYSTEM_FILES), filePath))
                {

                    #region SystemFilesIndex

                    switch (filePath)
                    {
                        case "MFT": 
                        //case "$MFT":
                            index = (int)SYSTEM_FILES.MFT;
                            break;
                        case "MFTMirr": 
                        //case "$MFTMirr":
                            index = (int)SYSTEM_FILES.MFTMirr;
                            break;
                        case "LogFile": 
                        //case "$LogFile":
                            index = (int)SYSTEM_FILES.LogFile;
                            break;
                        case "Volume": 
                        //case "$Volume":
                            index = (int)SYSTEM_FILES.Volume;
                            break;
                        case "AttrDef": 
                        //case "$AttrDef":
                            index = (int)SYSTEM_FILES.AttrDef;
                            break;
                        case "Root": 
                        //case "$Root":
                            index = (int)SYSTEM_FILES.Root;
                            break;
                        case "Bitmap": 
                        //case "$Bitmap":
                            index = (int)SYSTEM_FILES.Bitmap;
                            break;
                        case "Boot": 
                        //case "$Boot":
                            index = (int)SYSTEM_FILES.Boot;
                            break;
                        case "BadClus": 
                        //case "$BadClus":
                            index = (int)SYSTEM_FILES.BadClus;
                            break;
                        case "Secure": 
                        //case "$Secure":
                            index = (int)SYSTEM_FILES.Secure;
                            break;
                        case "UpCase": 
                        //case "$UpCase":
                            index = (int)SYSTEM_FILES.UpCase;
                            break;
                        case "Extend": 
                        //case "$Extend":
                            index = (int)SYSTEM_FILES.Extend;
                            break;
                        case "Quota": 
                        //case "$Quota":
                            index = (int)SYSTEM_FILES.Quota;
                            break;
                        case "ObjId": 
                        //case "$ObjId":
                            index = (int)SYSTEM_FILES.ObjId;
                            break;
                        case "Reparse": 
                        //case "$Reparse":
                            index = (int)SYSTEM_FILES.Reparse;
                            break;
                        case "RmMetadata": 
                        //case "$RmMetadata":
                            index = (int)SYSTEM_FILES.RmMetadata;
                            break;
                        case "Repair": 
                        //case "$Repair":
                            index = (int)SYSTEM_FILES.Repair;
                            break;
                        case "TxfLog": 
                        //case "$TxfLog":
                            index = (int)SYSTEM_FILES.TxfLog;
                            break;
                        case "Txf": 
                        //case "$Txf":
                            index = (int)SYSTEM_FILES.Txf;
                            break;
                        case "Tops": 
                        //case "$Tops":
                            index = (int)SYSTEM_FILES.Tops;
                            break;
                        case "TxfLog.blf": 
                        //case "$TxfLog.blf":
                            index = (int)SYSTEM_FILES.TxfLogblf;
                            break;
                        case "TxfLogContainer00000000000000000001": 
                        //case "$TxfLogContainer00000000000000000001":
                            index = (int)SYSTEM_FILES.TxfLogContainer00000000000000000001;
                            break;
                        case "TxfLogContainer00000000000000000002": 
                        //case "$TxfLogContainer00000000000000000002":
                            index = (int)SYSTEM_FILES.TxfLogContainer00000000000000000002;
                            break;
                    }

                    #endregion SystemFilesIndex

                }

                else
                {
                    index = InvokeIR.PowerForensics.NTFS.MFT.IndexNumber.Get(volume, filePath);
                }
                
                if (asbytes)
                {
                    WriteObject(MFTRecord.getMFTRecordBytes(mftBytes, index));
                }

                else
                {
                    WriteObject(MFTRecord.Get(mftBytes, index));
                }
            }

            else if(this.MyInvocation.BoundParameters.ContainsKey("IndexNumber"))
            {
                if (asbytes)
                {
                    WriteObject(MFTRecord.getMFTRecordBytes(mftBytes, indexNumber));
                }

                else
                {
                    WriteObject(MFTRecord.Get(mftBytes, indexNumber));
                }
            }

            else
            {
                MFTRecord[] records = MFTRecord.GetInstances(mftBytes);
                
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
