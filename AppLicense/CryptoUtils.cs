using System;
using System.Security.Cryptography;

namespace AppLicense
{
    internal class CryptoUtils
    {
        private const string publicKeyXmlString = "<RSAKeyValue><Modulus>q/0QiPA17+OvT8EAGzLImzCYUI5FmwXCreoeDoBtvHcX+VsYxviRoqsQ1ZO2e9hosDmX8m7A41TVQk6nGhGkKPjcQBRLpHDYbMvwGgsbWyCOdFbMhbKyLYjDhd7LkIGCqKn7bpauQHD5uzpBrUwZY80pjc5iHwzAK/A7Qbjm2O0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string hashAlgorithmName = "SHA1";

        public static string GenerateSignature(string machineId, string privateKeyXmlString, out string errorMessage)
        {
            errorMessage = null;
            string signature = null;

            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(privateKeyXmlString);
                    signedBytes = rsa.SignData(StringToBytes(machineId), CryptoConfig.MapNameToOID(hashAlgorithmName));
                    signature = BytesToString(signedBytes);
                }
                catch (Exception exception)
                {
                    errorMessage = exception.Message;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return signature;
        }

        public static bool VerifyData(string toVerify, string signature)
        {
            bool result = false;

            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(publicKeyXmlString);
                    result = rsa.VerifyData(StringToBytes(toVerify),
                                CryptoConfig.MapNameToOID(hashAlgorithmName), StringToBytes(signature));
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            
            return result;
        }

        private static byte[] StringToBytes(string s)
        {
            return Convert.FromBase64String(s);
        }

        private static string BytesToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
