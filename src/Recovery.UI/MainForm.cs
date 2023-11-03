using Recovery.Core.Models;
using Recovery.Core.Dataloaders;
using Recovery.Core.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Recovery.UI
{
    /// <summary>
    /// This class represents the main form of the application which is responsible for displaying lost X-rays and allowing the user to recover them.
    /// </summary>
    public partial class MainForm : Form
    {
        // List to store the lost X-rays found.
        private List<LostXray>? _lostXrays;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Load += MainForm_Load;
        }

        /// <summary>
        /// Event handler for the Load event of the MainForm. 
        /// It initializes the default image for the PictureBox, 
        /// shows the terms of use to the user, and fetches the lost X-rays upon user agreement.
        /// </summary>
        private void MainForm_Load(object? sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Welcome to the Carestream Xray Recovery Tool.\n\n" +
                "How to use:\n" +
                "1. Lost Xray images will be loaded into the list upon agreeing to the terms.\n" +
                "2. Select an Xray from the list to preview the image.\n" +
                "3. To recover a selected Xray, click the 'Recover' button. The image will be recovered to your desktop.\n\n" +
                "Disclaimer:\n" +
                "This tool is designed for emergency recovery situations. If you find yourself needing to use this tool frequently, " +
                "it may indicate an underlying issue with your system or software. We strongly advise contacting your Carestream support " +
                "representative to address and rectify the root cause, ensuring the safety and integrity of your medical data.\n\n" +
                "Do you agree to these terms and wish to search for lost Xrays?",
                "Terms of Use", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dialogResult == DialogResult.Yes)
            {
                // Load the default image on form initialization.
                PreviewBox.Image = Properties.Resources.CS7600;

                // Instantiate the X-ray finder.
                CS7600XrayFinder xrayFinder = new CS7600XrayFinder();

                // Fetch lost X-rays.
                FinderResult finderResult = xrayFinder.FindXrays();

                // If there's an error or no X-rays are found, display an error message.
                if (!finderResult.Success)
                {
                    MessageBox.Show(finderResult.ErrorMessage);
                    return;
                }

                // Populate the ListBox with found X-rays.
                _lostXrays = finderResult.LostXrays;
                foreach (var xray in _lostXrays)
                {
                    // Update the date format to include time up to seconds.
                    XrayListBox.Items.Add($"{xray.PatientFirstName} {xray.PatientLastName}, Date Taken: {xray.DateTaken:yyyy-MM-dd HH:mm:ss}");
                }
            }
            else
            {
                // Optional: Close the application if the user does not agree.
                this.Close();
            }
        }

        /// <summary>
        /// Event handler for when an item is selected in the XrayListBox. 
        /// It updates the PreviewBox with the image of the selected X-ray.
        /// </summary>
        private void XrayListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int selectedIndex = XrayListBox.SelectedIndex;
            if (selectedIndex != -1 && _lostXrays != null && _lostXrays.Count > selectedIndex)
            {
                var selectedXray = _lostXrays[selectedIndex];

                // Verify that the image file exists.
                if (System.IO.File.Exists(selectedXray.PreviewImagePath))
                {
                    PreviewBox.Image = Image.FromFile(selectedXray.PreviewImagePath);
                }
                else
                {
                    MessageBox.Show($"Failed to load preview image from the path: {selectedXray.PreviewImagePath}");
                    PreviewBox.Image = Properties.Resources.CS7600; // Default to initial image if there's an error.
                }
            }
        }

        /// <summary>
        /// Event handler for the click event of the Recover button. 
        /// It tries to recover the selected X-ray and provides feedback to the user.
        /// </summary>
        private void RecoverBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex = XrayListBox.SelectedIndex;
            if (selectedIndex != -1 && _lostXrays != null && _lostXrays.Count > selectedIndex)
            {
                // Instantiate the RecoveryService.
                RecoveryService recoveryService = new RecoveryService();
                var selectedXray = _lostXrays[selectedIndex];
                bool isRecovered = recoveryService.RecoverLostXray(selectedXray);

                // Notify the user about the outcome of the recovery attempt.
                if (isRecovered)
                {
                    MessageBox.Show($"Successfully recovered X-ray for: {selectedXray.PatientFirstName} {selectedXray.PatientLastName}, Date Taken: {selectedXray.DateTaken:yyyy-MM-dd HH:mm:ss}");
                }
                else
                {
                    MessageBox.Show($"Failed to recover X-ray for: {selectedXray.PatientFirstName} {selectedXray.PatientLastName}, Date Taken: {selectedXray.DateTaken:yyyy-MM-dd HH:mm:ss}");
                }
            }
            else
            {
                MessageBox.Show("Please select a patient's X-ray from the list to recover.");
            }
        }
    }
}
