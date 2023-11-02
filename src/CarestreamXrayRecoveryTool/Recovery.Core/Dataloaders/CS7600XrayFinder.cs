using Recovery.Core.Models;
using Recovery.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Recovery.Core.Dataloaders
{
    public class CS7600XrayFinder
    {
        private const string StudiesPath32 = @"C:\Program Files (x86)\CSH\CS7600\images\studies";
        private const string StudiesPath64 = @"C:\Program Files\CSH\CS7600\images\studies";

        public FinderResult FindXrays()
        {
            var result = new FinderResult();
            var studiesDirectory = GetStudiesDirectoryPath();

            if (string.IsNullOrEmpty(studiesDirectory))
            {
                result.Success = false;
                result.ErrorMessage = "Failed to locate the default directory.";
                return result;
            }

            result.LostXrays = FindFromStudiesDirectory(studiesDirectory);
            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.Success = true;
            }
            return result;
        }

        private string? GetStudiesDirectoryPath()
        {
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

        private List<LostXray> FindFromStudiesDirectory(string studiesFolderPath)
        {
            List<LostXray> allLostXrays = new List<LostXray>();
            try
            {
                var patientDirectories = Directory.GetDirectories(studiesFolderPath);

                foreach (var patientFolderPath in patientDirectories)
                {
                    var lostXraysForPatient = FindFromPatientFolder(patientFolderPath);
                    allLostXrays.AddRange(lostXraysForPatient);
                }
            }
            catch (Exception ex)
            {
                return new FinderResult { ErrorMessage = $"Error encountered while processing the studies directory at {studiesFolderPath}: {ex.Message}" }.LostXrays;
            }
            return allLostXrays;
        }

        private List<LostXray> FindFromPatientFolder(string patientFolderPath)
        {
            List<LostXray> lostXrays = new List<LostXray>();
            try
            {
                string inputDataPath = Path.Combine(patientFolderPath, "InputData.xml");

                if (!File.Exists(inputDataPath))
                {
                    return lostXrays;
                }

                XElement inputDataXml = XElement.Load(inputDataPath);

                string? patientFirstName = inputDataXml.Element("Patient_FirstName")?.Value.Trim();
                string? patientLastName = inputDataXml.Element("Patient_LastName")?.Value.Trim();

                if (string.IsNullOrWhiteSpace(patientFirstName) || string.IsNullOrWhiteSpace(patientLastName))
                {
                    return lostXrays;
                }

                var xraySubDirectories = Directory.GetDirectories(patientFolderPath);

                foreach (var xraySubDir in xraySubDirectories)
                {
                    string? lostXrayPath = Directory.GetFiles(xraySubDir, "U*.dcm").FirstOrDefault();
                    DateTime? dateTaken = lostXrayPath != null ? File.GetLastWriteTime(lostXrayPath) : (DateTime?)null;
                    string? previewImagePath = Directory.GetFiles(xraySubDir, "*.jpg").FirstOrDefault();

                    if (lostXrayPath != null && previewImagePath != null && dateTaken.HasValue)
                    {
                        var lostXray = new LostXray(patientFirstName, patientLastName, dateTaken.Value, previewImagePath, lostXrayPath);
                        lostXrays.Add(lostXray);
                    }
                }
            }
            catch (Exception ex)
            {
                return new FinderResult { ErrorMessage = $"Error encountered while processing the patient directory at {patientFolderPath}: {ex.Message}" }.LostXrays;
            }
            return lostXrays;
        }
    }
}
