using AppLicense;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace LicenseSandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var licensePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "license.json");

            // Generate license
            string privateKeyXmlString = "";
            var machineId = AppLicenseManager.GetMachineId();
            string errorMessage;
            LicenseData licenseData = AppLicenseManager.GenerateLicenseData(machineId, privateKeyXmlString, out errorMessage);

            // Write the license to a file
            string licenseJsonString = JsonSerializer.Serialize(licenseData);
            File.WriteAllText(licensePath, licenseJsonString);

            // Verify license
            string invalidReason;
            var isLicenseValid = AppLicenseManager.IsLicenseValid(licensePath, out invalidReason);
        }
    }
}
