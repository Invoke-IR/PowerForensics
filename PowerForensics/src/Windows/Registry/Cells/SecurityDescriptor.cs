using System;

namespace PowerForensics.Registry
{
    #region SecurityDescriptorClass

    public class SecurityDescriptor
    {
        #region Enums

        [FlagsAttribute]
        public enum SECURITY_KEY_CONTROLS
        {
            SeOwnerDefaulted = 0x0001,
            SeGroupDefaulted = 0x0002,
            SeDaclPresent = 0x0004,
            SeDaclDefaulted = 0x0008,
            SeSaclPresent = 0x0010,
            SeSaclDefaulted = 0x0020,
            SeDaclAutoInheritReq = 0x0100,
            SeSaclAutoInheritReq = 0x0200,
            SeDaclAutoInherited = 0x0400,
            SeSaclAutoInherited = 0x0800,
            SeDaclProtected = 0x1000,
            SeSaclProtected = 0x2000,
            SeRmControlValid = 0x4000,
            SeSelfRelative = 0x8000,
        }

        #endregion Enums
        
        #region Properties

        public readonly SECURITY_KEY_CONTROLS Control;
        internal readonly uint OwnerOffset;
        internal readonly uint GroupOffset;
        internal readonly uint SACLOffset;
        internal readonly uint DACLOffset;
        public readonly byte[] Owner;
        public readonly byte[] Group;
        public readonly byte[] SACL;
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
    
    #endregion SecurityDescriptorClass
}
