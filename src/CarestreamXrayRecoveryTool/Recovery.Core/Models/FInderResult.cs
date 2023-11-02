namespace Recovery.Core.Models
{
    public class FinderResult
    {
        /// <summary>
        /// Indicates whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// List of lost xrays found during the operation.
        /// </summary>
        public List<LostXray> LostXrays { get; set; } = new List<LostXray>();

        /// <summary>
        /// Any error message encountered during the operation.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
