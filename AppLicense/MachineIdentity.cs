using System.Management;

namespace AppLicense
{
    public static class MachineIdentity
    {
        internal static string getCpuId()
        {
            var processorClassName = "Win32_Processor";
            string result = "";

            // Use UniqueId
            result = getIdentifier(processorClassName, "UniqueId");
            if (!string.IsNullOrEmpty(result)) { return result; }

            //If no UniqueID, use ProcessorID
            result = getIdentifier(processorClassName, "ProcessorId");
            if (!string.IsNullOrEmpty(result)) { return result; }

            return result;
        }

        // General method to get hardware value
        private static string getIdentifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            ManagementClass managementClass = new ManagementClass(wmiClass);
            var instances = managementClass.GetInstances();

            foreach (ManagementObject instance in instances)
            {
                try
                {
                    result = instance.Properties[wmiProperty].Value.ToString();
                    break;
                }
                catch { continue; }
            }

            return result;
        }
    }
}
