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
                case "SHA1":
                    return new SHA1CryptoServiceProvider();
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
            return BitConverter.ToString(hashAlgorithm.ComputeHash(Helper.GetSubArray(bytes, 0x00, count))).Replace("-", "");
        }

        #endregion StaticMethods
    }

    #endregion HashClass
}