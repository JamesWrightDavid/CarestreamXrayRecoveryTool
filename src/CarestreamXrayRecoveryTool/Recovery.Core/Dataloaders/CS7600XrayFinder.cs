using Recovery.Core.Models;
using Recovery.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Recovery.Core.Dataloaders
{
    /// <summary>
    /// The CS7600XrayFinder class is responsible for searching and retrieving lost X-rays from 
    /// specific directories on the system. It is designed to work on both 32-bit and 64-bit 
    /// installations of the software.
    /// </summary>
    public class CS7600XrayFinder
    {
        // Constants to define the directories where X-ray studies might be located. 
        // These paths are dependent on the architecture of the system (32 or 64 bit).
        private const string StudiesPath32 = @"C:\Program Files (x86)\CSH\CS7600\images\studies";
        private const string StudiesPath64 = @"C:\Program Files\CSH\CS7600\images\studies";

        /// <summary>
        /// This method initiates the search for lost X-rays and returns the findings in 
        /// a structured format.
        /// </summary>
        /// <returns>A FinderResult object containing the search result and any lost X-rays found.</returns>
        public FinderResult FindXrays()
        {
            var result = new FinderResult();

            // First, determine the correct path to the studies directory based on system architecture.
            var studiesDirectory = GetStudiesDirectoryPath();

            // If we couldn't determine the path, something's wrong.
            if (string.IsNullOrEmpty(studiesDirectory))
            {
                result.Success = false;
                result.ErrorMessage = "Failed to locate the studies directory.";
                return result;
            }

            // Fetch any lost X-rays from the studies directory.
            result.LostXrays = FindFromStudiesDirectory(studiesDirectory);

            // If no X-rays are found, we want to provide a clear error message.
            if (result.LostXrays.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "No lost X-rays found in the studies directory.";
                return result;
            }

            // If no errors encountered during the process, mark the search as successful.
            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.Success = true;
            }
            return result;
        }

        /// <summary>
        /// Determines the correct studies directory based on whether the system 
        /// has a 32-bit or 64-bit installation of the software.
        /// </summary>
        /// <returns>The path to the studies directory or null if none exists.</returns>
        private string? GetStudiesDirectoryPath()
        {
            // Check for existence of directories and return the appropriate path.
            if (Directory.Exists(StudiesPath32))
            {
                return StudiesPath32;
            }
            else if (Directory.Exists(StudiesPath64))
            {
                return StudiesPath64;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Searches the provided studies directory for any lost X-rays.
        /// </summary>
        /// <param name="studiesFolderPath">The path to the studies directory.</param>
        /// <returns>A list of lost X-rays found within the studies directory.</returns>
        private List<LostXray> FindFromStudiesDirectory(string studiesFolderPath)
        {
            List<LostXray> allLostXrays = new List<LostXray>();

            try
            {
                // Retrieve all patient directories within the studies directory.
                var patientDirectories = Directory.GetDirectories(studiesFolderPath);

                // Iterate through each patient directory to search for X-rays.
                foreach (var patientFolderPath in patientDirectories)
                {
                    var lostXraysForPatient = FindFromPatientFolder(patientFolderPath);
                    allLostXrays.AddRange(lostXraysForPatient);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors encountered during the process.
                return new FinderResult { ErrorMessage = $"Error encountered while processing the studies directory at {studiesFolderPath}: {ex.Message}" }.LostXrays;
            }
            return allLostXrays;
        }

        /// <summary>
        /// Searches a patient's folder for any lost X-rays.
        /// </summary>
        /// <param name="patientFolderPath">The path to a patient's directory.</param>
        /// <returns>A list of lost X-rays found within the patient's directory.</returns>
        private List<LostXray> FindFromPatientFolder(string patientFolderPath)
        {
            List<LostXray> lostXrays = new List<LostXray>();

            try
            {
                // Locate the XML file containing patient details within the directory.
                string inputDataPath = Path.Combine(patientFolderPath, "InputData.xml");

                // If the XML file doesn't exist, we can't proceed further for this patient.
                if (!File.Exists(inputDataPath))
                {
                    return lostXrays;
                }

                // Load the XML content to fetch patient details.
                XElement inputDataXml = XElement.Load(inputDataPath);

                string? patientFirstName = inputDataXml.Element("Patient_FirstName")?.Value.Trim();
                string? patientLastName = inputDataXml.Element("Patient_LastName")?.Value.Trim();

                if (string.IsNullOrWhiteSpace(patientFirstName) || string.IsNullOrWhiteSpace(patientLastName))
                {
                    return lostXrays;
                }

                // Search for any subdirectories within the patient's directory, 
                // each potentially containing an X-ray.
                var xraySubDirectories = Directory.GetDirectories(patientFolderPath);

                foreach (var xraySubDir in xraySubDirectories)
                {
                    // Look for any lost X-ray file and its associated preview image.
                    string? lostXrayPath = Directory.GetFiles(xraySubDir, "U*.dcm").FirstOrDefault();
                    DateTime? dateTaken = lostXrayPath != null ? File.GetLastWriteTime(lostXrayPath) : (DateTime?)null;
                    string? previewImagePath = Directory.GetFiles(xraySubDir, "*.jpg").FirstOrDefault();

                    if (lostXrayPath != null && previewImagePath != null && dateTaken.HasValue)
                    {
                        // Create an object representation of the found X-ray.
                        var lostXray = new LostXray(patientFirstName, patientLastName, dateTaken.Value, previewImagePath, lostXrayPath);
                        lostXrays.Add(lostXray);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors encountered during the process.
                return new FinderResult { ErrorMessage = $"Error encountered while processing the patient directory at {patientFolderPath}: {ex.Message}" }.LostXrays;
            }
            return lostXrays;
        }
    }
}
