namespace Recovery.Core.Models
{
    /// <summary>
    /// Represents a lost X-ray in the recovery process.
    /// This class captures essential details about a lost X-ray,
    /// including the associated patient's name, the date the X-ray was taken,
    /// the path to the preview image, and the path to the lost X-ray itself.
    /// It is used in the process of scanning directories for X-ray data and
    /// presenting that data in a user-friendly format.
    /// </summary>
    public class LostXray
    {
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime DateTaken { get; set; }

        /// <summary>
        /// Property to hold the path of the preview image (JPG file).
        /// </summary>
        public string PreviewImagePath { get; set; }

        /// <summary>
        /// Property to hold the path of the lost X-ray (.DCM file starting with U).
        /// </summary>
        public string LostXrayPath { get; set; }

        public string FullName => $"{PatientFirstName} {PatientLastName}";

        public override string ToString()
        {
            return $"{FullName} - {DateTaken:yyyy-MM-dd HH:mm:ss}";
        }

        /// <summary>
        /// Constructor to create a new instance of the LostXray class.
        /// It accepts five parameters: patientFirstName, patientLastName, dateTaken, previewImagePath, and lostXrayPath.
        /// ArgumentNullException is thrown if patientFirstName, patientLastName, previewImagePath, or lostXrayPath is null.
        /// </summary>
        public LostXray(string patientFirstName, string patientLastName, DateTime dateTaken, string previewImagePath, string lostXrayPath)
        {
            PatientFirstName = patientFirstName ?? throw new ArgumentNullException(nameof(patientFirstName));
            PatientLastName = patientLastName ?? throw new ArgumentNullException(nameof(patientLastName));
            DateTaken = dateTaken;
            PreviewImagePath = previewImagePath ?? throw new ArgumentNullException(nameof(previewImagePath));
            LostXrayPath = lostXrayPath ?? throw new ArgumentNullException(nameof(lostXrayPath));
        }
    }
}
