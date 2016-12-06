using System;

namespace PowerForensics.Windows.Registry
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityDescriptor
    {
        #region Enums

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        public enum SECURITY_KEY_CONTROLS
        {
            /// <summary>
            /// 
            /// </summary>
            SeOwnerDefaulted = 0x0001,

            /// <summary>
            /// 
            /// </summary>
            SeGroupDefaulted = 0x0002,

            /// <summary>
            /// 
            /// </summary>
            SeDaclPresent = 0x0004,

            /// <summary>
            /// 
            /// </summary>
            SeDaclDefaulted = 0x0008,

            /// <summary>
            /// 
            /// </summary>
            SeSaclPresent = 0x0010,

            /// <summary>
            /// 
            /// </summary>
            SeSaclDefaulted = 0x0020,

            /// <summary>
            /// 
            /// </summary>
            SeDaclAutoInheritReq = 0x0100,

            /// <summary>
            /// 
            /// </summary>
            SeSaclAutoInheritReq = 0x0200,

            /// <summary>
            /// 
            /// </summary>
            SeDaclAutoInherited = 0x0400,

            /// <summary>
            /// 
            /// </summary>
            SeSaclAutoInherited = 0x0800,

            /// <summary>
            /// 
            /// </summary>
            SeDaclProtected = 0x1000,

            /// <summary>
            /// 
            /// </summary>
            SeSaclProtected = 0x2000,

            /// <summary>
            /// 
            /// </summary>
            SeRmControlValid = 0x4000,

            /// <summary>
            /// 
            /// </summary>
            SeSelfRelative = 0x8000,
        }

        #endregion Enums
        
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly SECURITY_KEY_CONTROLS Control;

        internal readonly uint OwnerOffset;

        internal readonly uint GroupOffset;

        internal readonly uint SACLOffset;

        internal readonly uint DACLOffset;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] Owner;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] Group;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] SACL;

        /// <summary>
        /// 
        /// </summary>
        public readonly byte[] DACL;

        #endregion Properties

        #region Constructors

        internal SecurityDescriptor(byte[] bytes)
        {
            Control = (SECURITY_KEY_CONTROLS)BitConverter.ToUInt16(bytes, 0x02);
            OwnerOffset = BitConverter.ToUInt32(bytes, 0x04);
            GroupOffset = BitConverter.ToUInt32(bytes, 0x08);
            SACLOffset = BitConverter.ToUInt32(bytes, 0x0C);
            DACLOffset = BitConverter.ToUInt32(bytes, 0x10);
            Owner = Helper.GetSubArray(bytes, (int)OwnerOffset, 0x10);
            Group = Helper.GetSubArray(bytes, (int)GroupOffset, 0x0C);
            SACL = Helper.GetSubArray(bytes, (int)SACLOffset, 0x08);
            DACL = Helper.GetSubArray(bytes, (int)DACLOffset, 0x84);
        }

        #endregion Constructors
    }
}