using System;
using System.Security.Cryptography;

namespace PowerForensics.Utilities
{
    #region HashClass

    class Hash
    {
        #region StaticMethods

        private static HashAlgorithm GetAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "MD5":
                    return new MD5CryptoServiceProvider();
                case "RIPEMD160":
                    return RIPEMD160Managed.Create();
                case "SHA1":
                    return new SHA1CryptoServiceProvider();
                case "SHA256":
                    return new SHA256CryptoServiceProvider();
                case "SHA384":
                    return new SHA384CryptoServiceProvider();
                case "SHA512":
                    return new SHA512CryptoServiceProvider();
                default:
                    throw new Exception("Invalid Hash Algorithm Provided");
            }
        }

        internal static string Get(byte[] bytes, string algorithm)
        {
            // Create a hash algorithm for specified algorithm
            HashAlgorithm hashAlgorithm = GetAlgorithm(algorithm);

            //Output the computed MD5 Hash as a string to the PowerShell pipeline
            return BitConverter.ToString(hashAlgorithm.ComputeHash(bytes)).Replace("-", "");
        }

        internal static string Get(byte[] bytes, int count, string algorithm)
        {
            // Create a hash algorithm for specified algorithm
            HashAlgorithm hashAlgorithm = GetAlgorithm(algorithm);

            //Output the computed MD5 Hash as a string to the PowerShell pipeline
            return BitConverter.ToString(hashAlgorithm.ComputeHash(Util.GetSubArray(bytes, 0x00, (uint)count))).Replace("-", "");
        }

        #endregion StaticMethods
    }

    #endregion HashClass
}