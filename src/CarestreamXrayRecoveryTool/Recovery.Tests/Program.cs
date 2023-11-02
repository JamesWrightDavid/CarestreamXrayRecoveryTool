using Recovery.Core.Models;
using Recovery.Core.Dataloaders;
using Recovery.Core.Services;
using System;
using System.Collections.Generic;

namespace CS7600XrayRecover.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Provide the initial prompt to the user for searching X-rays
            Console.WriteLine("Do you want to search for lost X-rays? (yes/no)");
            string response = Console.ReadLine().ToLower();

            // If the user's response isn't affirmative, exit the program
            if (response != "yes")
            {
                Console.WriteLine("Exiting the program.");
                EndProgram();
                return;
            }

            // Creating an instance of the CS7600XrayFinder to search for lost X-rays
            CS7600XrayFinder xrayFinder = new CS7600XrayFinder();

            // Fetching the lost X-ray data for testing
            FinderResult finderResult = xrayFinder.FindXrays();

            // If no X-rays found or an error occurred, display the error message and exit
            if (!finderResult.Success)
            {
                Console.WriteLine(finderResult.ErrorMessage);
                EndProgram();
                return;
            }

            // Display the lost X-rays found to the user
            Console.WriteLine("Lost X-rays found:");
            DisplayLostXrays(finderResult.LostXrays);

            // Prompt the user to select a specific X-ray for recovery
            LostXray selectedXray = GetUserSelection(finderResult.LostXrays);

            // Initiate the recovery process for the selected X-ray
            RecoverSelectedXray(selectedXray);

            // Wait for user input before closing the console application
            EndProgram();
        }

        /// <summary>
        /// Display the list of lost X-rays to the user.
        /// </summary>
        /// <param name="lostXrays">List of lost X-rays</param>
        private static void DisplayLostXrays(List<LostXray> lostXrays)
        {
            foreach (var xray in lostXrays)
            {
                Console.WriteLine($"- Patient: {xray.PatientFirstName} {xray.PatientLastName}, Date Taken: {xray.DateTaken:yyyy-MM-dd}, Preview Image Path: {xray.PreviewImagePath}, Lost X-ray Path: {xray.LostXrayPath}");
            }
        }

        /// <summary>
        /// Prompt the user to select a specific X-ray for recovery.
        /// </summary>
        /// <param name="lostXrays">List of lost X-rays</param>
        /// <returns>The selected X-ray</returns>
        private static LostXray GetUserSelection(List<LostXray> lostXrays)
        {
            Dictionary<int, LostXray> patientChoices = new Dictionary<int, LostXray>();

            Console.WriteLine("\nPlease select a patient by number for X-ray recovery:");

            int choiceNumber = 1;
            foreach (var xray in lostXrays)
            {
                Console.WriteLine($"{choiceNumber}. Patient: {xray.PatientFirstName} {xray.PatientLastName}, Date Taken: {xray.DateTaken:yyyy-MM-dd}");
                patientChoices[choiceNumber] = xray;
                choiceNumber++;
            }

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int selectedNumber) && patientChoices.ContainsKey(selectedNumber))
                {
                    return patientChoices[selectedNumber];
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please select a valid number from the list above.");
                }
            }
        }

        /// <summary>
        /// Initiates the recovery process for the selected X-ray.
        /// </summary>
        /// <param name="selectedXray">The selected X-ray for recovery</param>
        private static void RecoverSelectedXray(LostXray selectedXray)
        {
            // Using the RecoveryService to handle the recovery process
            RecoveryService recoveryService = new RecoveryService();
            bool isRecovered = recoveryService.RecoverLostXray(selectedXray);

            // Notify the user about the success or failure of the recovery process
            if (isRecovered)
            {
                Console.WriteLine($"Successfully recovered X-ray for: {selectedXray.PatientFirstName} {selectedXray.PatientLastName}, Date Taken: {selectedXray.DateTaken:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine($"Failed to recover X-ray for: {selectedXray.PatientFirstName} {selectedXray.PatientLastName}, Date Taken: {selectedXray.DateTaken:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// A utility method to wait for user input before closing the console application.
        /// </summary>
        private static void EndProgram()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
