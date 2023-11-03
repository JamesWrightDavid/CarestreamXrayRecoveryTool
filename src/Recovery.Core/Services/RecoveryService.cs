using Recovery.Core.Models;
using Recovery.Core.Helpers;
using System;
using System.IO;

namespace Recovery.Core.Services
{
    public class RecoveryService
    {
        // Define the location of the main recovery folder on the user's desktop.
        // This will hold all recovered X-rays, organized by patient name.
        private readonly string mainRecoveryFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "recovered X-rays");
        private readonly PathSanitizer sanitizer = new PathSanitizer();  // Add this line to initialize the Sanitizer helper class.

        // Constructor
        public RecoveryService()
        {
            // Check if the main recovery folder already exists on the desktop.
            // If it doesn't, create it. This ensures we have a base directory to work with.
            if (!Directory.Exists(mainRecoveryFolder))
            {
                Directory.CreateDirectory(mainRecoveryFolder);
            }
        }

        // Method to recover a lost X-ray.
        public bool RecoverLostXray(LostXray lostXray)
        {
            try
            {
                // Remove any characters from the patient's name that are invalid for file/directory names.
                // Use the Sanitizer helper class's method for this.
                string sanitizedFirstName = sanitizer.SanitizeFileName(lostXray.PatientFirstName);
                string sanitizedLastName = sanitizer.SanitizeFileName(lostXray.PatientLastName);

                // Construct the path for the specific patient's subfolder within the main recovery folder.
                string patientSubFolder = Path.Combine(mainRecoveryFolder, $"{sanitizedFirstName} {sanitizedLastName}");

                // If the specific patient's subfolder doesn't exist, create it.
                if (!Directory.Exists(patientSubFolder))
                {
                    Directory.CreateDirectory(patientSubFolder);
                }

                // Format the filename for the recovered X-ray using sanitized names and the date the X-ray was taken.
                string destinationFileName = $"{sanitizedFirstName} {sanitizedLastName} {lostXray.DateTaken:yyyy-MM-dd HH-mm-ss}.dcm";
                string destinationFilePath = Path.Combine(patientSubFolder, destinationFileName);

                // Copy the lost X-ray to the destination. If a file with the same name already exists, overwrite it.
                File.Copy(lostXray.LostXrayPath, destinationFilePath, overwrite: true);

                return true; // Indicate that the recovery was successful.
            }
            catch (Exception ex)
            {
                // Log any exceptions that occurred during the recovery process. This could be useful for debugging.
                Console.WriteLine($"Error recovering x-ray for {lostXray.FullName}: {ex.Message}");
                return false; // Indicate that the recovery failed.
            }
        }
    }
}
