using Recovery.Core.Models;
using Recovery.Core.Dataloaders;   // Namespace for CS7600XrayFinder
using Recovery.Core.Services;
using System;
using System.Collections.Generic;

namespace CS7600XrayRecover.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating an instance of the new CS7600XrayFinder
            CS7600XrayFinder xrayFinder = new CS7600XrayFinder();

            // Fetching the lost X-ray data for testing
            FinderResult finderResult = xrayFinder.FindXrays();

            // Check if any lost X-rays were found
            if (!finderResult.Success || finderResult.LostXrays.Count == 0)
            {
                Console.WriteLine(finderResult.ErrorMessage ?? "No lost X-rays found.");
            }
            else
            {
                Console.WriteLine("Lost X-rays found:");
                foreach (var xray in finderResult.LostXrays)
                {
                    Console.WriteLine($"- Patient: {xray.PatientFirstName} {xray.PatientLastName}, Date Taken: {xray.DateTaken:yyyy-MM-dd}, Preview Image Path: {xray.PreviewImagePath}, Lost X-ray Path: {xray.LostXrayPath}");
                }

                // Creating a dictionary to map choice numbers to X-ray entries for user selection
                Dictionary<int, LostXray> patientChoices = new Dictionary<int, LostXray>();

                // Display a list of patients with lost X-rays and ask the user to select one by number
                Console.WriteLine("\nPlease select a patient by number for X-ray recovery:");

                int choiceNumber = 1;
                foreach (var xray in finderResult.LostXrays)
                {
                    Console.WriteLine($"{choiceNumber}. Patient: {xray.PatientFirstName} {xray.PatientLastName}, Date Taken: {xray.DateTaken:yyyy-MM-dd}");
                    patientChoices[choiceNumber] = xray;  // Map the choice number to the X-ray entry
                    choiceNumber++;
                }

                // Loop to ensure user makes a valid selection
                int selectedNumber;
                while (true)
                {
                    // Try to parse user input as an integer and check if it matches one of the choices
                    if (int.TryParse(Console.ReadLine(), out selectedNumber) && patientChoices.ContainsKey(selectedNumber))
                    {
                        break;  // Exit the loop once a valid choice is made
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please select a valid number from the list above.");
                    }
                }

                // Fetch the selected X-ray using the user's choice
                LostXray selectedXray = patientChoices[selectedNumber];

                // Initiate the RecoveryService
                RecoveryService recoveryService = new RecoveryService();

                // Use the RecoveryService to recover the selected X-ray
                bool isRecovered = recoveryService.RecoverLostXray(selectedXray);

                if (isRecovered)
                {
                    Console.WriteLine($"Successfully recovered X-ray for: {selectedXray.PatientFirstName} {selectedXray.PatientLastName}, Date Taken: {selectedXray.DateTaken:yyyy-MM-dd}");
                }
                else
                {
                    Console.WriteLine($"Failed to recover X-ray for: {selectedXray.PatientFirstName} {selectedXray.PatientLastName}, Date Taken: {selectedXray.DateTaken:yyyy-MM-dd}");
                }
            }

            // Wait for user input before closing the console application
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
