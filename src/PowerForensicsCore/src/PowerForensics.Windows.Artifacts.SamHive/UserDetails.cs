using System;
using PowerForensics.Windows.Registry;

namespace PowerForensics.Windows.Artifacts.SamHive
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDetail
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime LastLogon;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime PasswordLastSet;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime AccountExpires;

        /// <summary>
        /// 
        /// </summary>
        public readonly DateTime LastIncorrectPassword;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint RelativeIdentifier;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool AccountActive;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool PasswordRequired;

        /// <summary>
        /// 
        /// </summary>
        public readonly string CountryCode;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint InvalidPasswordCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly uint LogonCount;

        #endregion Properties

        #region Constructors

        private UserDetail(byte[] bytes, NamedKey nk)
        {
            ValueKey[] values = nk.GetValues(bytes);
            foreach (ValueKey vk in values)
            {

            }
        }

        #endregion Constructors

        #region Static Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserDetail Get()
        {
            return null;
        }

        #endregion Static Methods
    }
}
