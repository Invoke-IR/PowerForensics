using System;
using System.Security.Cryptography;

namespace InvokeIR
{
    class MD5Hash
    {
        internal static string Get(byte[] bytes, int count)
        {
            // Instantiate a byte array of length "count"
            byte[] arr = new byte[count];

            // Read bytes array into arr array
            Array.Copy(bytes, 0, arr, 0, count);

            // Instantiate an MD5 class object
            MD5 md5Hash = MD5.Create();

            //Output the computed MD5 Hash as a string to the PowerShell pipeline
            return BitConverter.ToString(md5Hash.ComputeHash(arr)).Replace("-", "");
        }
    }
}
