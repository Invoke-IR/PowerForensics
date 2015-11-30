using System;
using PowerForensics.Registry;

namespace PowerForensics.Artifacts
{
    public class UserDetail
    {
        #region Properties

        public readonly DateTime LastLogon;
        public readonly DateTime PasswordLastSet;
        public readonly DateTime AccountExpires;
        public readonly DateTime LastIncorrectPassword;
        public readonly uint RelativeIdentifier;
        public readonly bool AccountActive;
        public readonly bool PasswordRequired;
        public readonly string CountryCode;
        public readonly uint InvalidPasswordCount;
        public readonly uint LogonCount;

        #endregion Properties

        #region Constructors

        internal UserDetail(byte[] bytes, NamedKey nk)
        {
            ValueKey[] values = nk.GetValues(bytes);
            foreach (ValueKey vk in values)
            {
                switch (vk.Name)
                {

                }
            }
        }

        #endregion Constructors

        #region StaticMethods

        public static UserDetail Get()
        {
            return null;
        }

        #endregion StaticMethods
    }
}
