using AppLicense;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace LicenseGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += mainWindow_Loaded;
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate default private key XML string. Get from RSACryptoServiceProvider.ToXmlString(true)
            privateKeyTextBox.Text = "";
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            statusText.Text = "Generating license...";

            if (string.IsNullOrEmpty(machineIdTextBox.Text))
            {
                statusText.Text = "Machine ID is required";
                return;
            }

            if (string.IsNullOrEmpty(privateKeyTextBox.Text))
            {
                statusText.Text = "Private key is required";
                return;
            }

            string errorMessage;
            var licenseData = AppLicenseManager.GenerateLicenseData(
                machineIdTextBox.Text, privateKeyTextBox.Text, out errorMessage);

            if (null != errorMessage)
            {
                statusText.Text = $"Error while generating signature:{Environment.NewLine}{errorMessage}";
                return;
            }

            // The license data is generated. Save it
            statusText.Text = $"License data generated. Saving to file...";
            promptSave(licenseData);
        }

        private void promptSave(LicenseData licenseData)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "license";
            dialog.DefaultExt = ".json";
            dialog.Filter = "JSON file (.json)|*.json";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            bool? result = dialog.ShowDialog();

            if (true != result)
            {
                statusText.Text = $"License generated but not saved to file";
                return;
            }

            // Save to file
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };

                string jsonString = JsonSerializer.Serialize(licenseData, options);
                File.WriteAllText(dialog.FileName, jsonString);
                statusText.Text = $"License file saved to:{Environment.NewLine}{dialog.FileName}";
            }
            catch (Exception exception)
            {
                statusText.Text = $"Error while saving license file:{Environment.NewLine}{exception.Message}";
            }
        }
    }
}
