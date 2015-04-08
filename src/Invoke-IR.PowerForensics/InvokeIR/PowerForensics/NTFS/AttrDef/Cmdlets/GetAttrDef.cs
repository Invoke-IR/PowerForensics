using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text.RegularExpressions;
using InvokeIR.Win32;
using InvokeIR.PowerForensics.NTFS;

namespace InvokeIR.PowerForensics.Cmdlets
{

    #region GetAttrDefCommand
    /// <summary> 
    /// This class implements the Get-AttrDef cmdlet. 
    /// </summary> 

    [Cmdlet(VerbsCommon.Get, "AttrDef")]
    public class GetAttrDefCommand : PSCmdlet
    {

        #region Parameters

        /// <summary> 
        /// This parameter provides the Volume Name for the 
        /// AttrDef objects that will be returned.
        /// </summary> 

        [Parameter(Mandatory = true, Position = 0)]
        public string VolumeName
        {
            get { return volume; }
            set { volume = value; }
        }
        private string volume;

        #endregion Parameters

        #region Cmdlet Overrides

        /// <summary> 
        /// The ProcessRecord method calls AttrDef.GetInstances() 
        /// method to iterate through each AttrDef object on the specified volume.
        /// </summary> 
        protected override void ProcessRecord()
        {

            WriteObject(AttrDef.GetInstances(volume));

        } // ProcessRecord 

        #endregion Cmdlet Overrides

    } // End GetProcCommand class. 

    #endregion GetAttrDefCommand

}
