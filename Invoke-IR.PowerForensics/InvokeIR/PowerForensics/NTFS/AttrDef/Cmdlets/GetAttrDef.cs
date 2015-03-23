using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS.MFT.Attributes;

namespace InvokeIR.PowerForensics.NTFS.AttrDef
{

    #region GetAttrDefCommand
    /// <summary> 
    /// This class implements the Get-BootFile cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "AttrDef")]
    public class GetAttrDefCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the DriveName for the 
        /// Partition Table that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls ManagementClass.GetInstances() 
        /// method to iterate through each BindingObject on each system specified.
        /// </summary> 
        protected override void ProcessRecord()
        {

            Regex lettersOnly = new Regex("^[a-zA-Z]{1}$");

            if (lettersOnly.IsMatch(volume))
            {

                volume = @"\\.\" + volume + ":";

            }

            IntPtr hVolume = NativeMethods.getHandle(volume);
            FileStream streamToRead = NativeMethods.getFileStream(hVolume);

            NTFSVolumeData volData = NTFSVolumeData.Get(hVolume);
            
            MFT.MFTRecord record = MFT.MFTRecord.Get(volume, 4);

            List<byte> bytes = new List<byte>();
            
            foreach(Attr attr in record.Attribute)
            {
                if(attr.Name == "DATA")
                {
                    if(attr.NonResident)
                    {
                        NonResident data = attr as NonResident;
                        for(int i = 0; i < data.StartCluster.Length; i++)
                        {
                            ulong offset = data.StartCluster[i] * (ulong)volData.BytesPerCluster;
                            ulong length = (data.EndCluster[i] - data.StartCluster[i]) * (ulong)volData.BytesPerCluster;
                            byte[] byteRange = Win32.NativeMethods.readDrive(streamToRead, offset, length);
                            bytes.AddRange(byteRange);
                        }
                    }
                    else
                    {
                        Data data = attr as Data;
                        bytes.AddRange(data.RawData);
                    }
                }
            }

            for (int i = 0; (i < bytes.ToArray().Length) && (bytes.ToArray()[i] != 0); i += 160)
            {
                byte[] attrDefBytes = bytes.Skip(i).Take(160).ToArray();
                WriteObject(AttrDef.Get(attrDefBytes));
            }

            streamToRead.Close();

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetProcCommand class. 

    #endregion GetAttrDefCommand

}
