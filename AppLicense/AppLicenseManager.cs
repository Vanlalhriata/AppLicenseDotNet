using System.IO;
using System.Text.Json;

namespace AppLicense
{
    public static class AppLicenseManager
    {
        public static string GetMachineId()
        {
            // Use CPU id only
            var result = MachineIdentity.getCpuId();
            return result;
        }

        public static LicenseData GenerateLicenseData(string machineId, string privateKeyXmlString, out string errorMessage)
        {
            var signature = CryptoUtils.GenerateSignature(machineId, privateKeyXmlString, out errorMessage);

            var licenseData = new LicenseData()
            {
                MachineId = machineId,
                Signature = signature,
            };

            return licenseData;
        }

        public static bool IsLicenseValid(string licensePath, out string invalidReason)
        {
            invalidReason = null;

            string licenseText = readLicense(licensePath);
            if (string.IsNullOrEmpty(licenseText))
            {
                invalidReason = $"License not found or is empty. Expected in: {licensePath}.";
                return false;
            }

            var licenseData = JsonSerializer.Deserialize<LicenseData>(licenseText);
            if (null == licenseData)
            {
                invalidReason = $"License format is invalid.";
                return false;
            }

            if (!isLicenseValid(licenseData))
            {
                invalidReason = $"License verification not passed.";
                return false;
            }

            return true;
        }

        private static string readLicense(string licensePath)
        {
            string result = null;

            try
            {
                result = File.ReadAllText(licensePath);
            }
            catch
            {
                // null will be returned and the error will be handled.
            }

            return result;
        }

        private static bool isLicenseValid(LicenseData licenseData)
        {
            string actualMachineId = GetMachineId();
            var isValid = CryptoUtils.VerifyData(licenseData.MachineId, licenseData.Signature);

            return isValid;
        }
    }
}
