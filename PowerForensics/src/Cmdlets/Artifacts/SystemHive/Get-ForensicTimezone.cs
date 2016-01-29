using System.Management.Automation;
using PowerForensics.Artifacts;

namespace PowerForensics.Cmdlets
{
    #region GetTimezoneCommand

    /// <summary> 
    /// This class implements the Get-Timezone cmdlet. 
    /// </summary> 
    [Cmdlet(VerbsCommon.Get, "ForensicTimezone", DefaultParameterSetName = "ByVolume")]
    public class GetTimezoneCommand : PSCmdlet
    {
        #region Parameters

        /// <summary> 
        ///
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
        /// This parameter provides the the path of the Registry Hive to parse.
        /// </summary> 
        [Alias("Path")]
        [Parameter(Mandatory = true, ParameterSetName = "ByPath")]
        public string HivePath
        {
            get { return hivePath; }
            set { hivePath = value; }
        }
        private string hivePath;

        #endregion Parameters

        #region Cmdlet Overrides
        
        /// <summary> 
        /// 
        /// </summary>  
        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ByVolume":
                    WriteObject(Timezone.Get(volume), true);
                    break;
                case "ByPath":
                    WriteObject(Timezone.GetByPath(hivePath), true);
                    break;
            }
        }

        #endregion Cmdlet Overrides
    }
    
    #endregion GetTimezoneCommand
}
