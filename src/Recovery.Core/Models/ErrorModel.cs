namespace Recovery.Core.Models
{
    public class ErrorModel
    {
        /// <summary>
        /// Gets or sets the error code. This can be used to map to specific localized messages.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the default error message in English. 
        /// Ideally, this should be replaced with a localized message at the UI layer using the ErrorCode.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets any additional details about the error. This can be used for logging or debugging.
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Constructor for initializing the error model.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorMessage">The default error message.</param>
        /// <param name="details">Any additional details about the error.</param>
        public ErrorModel(string errorCode, string errorMessage, string? details = null)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
            Details = details;
        }

        /// <summary>
        /// Override ToString for better logging and debugging.
        /// </summary>
        /// <returns>A string representation of the error model.</returns>
        public override string ToString()
        {
            return $"Error Code: {ErrorCode}, Message: {ErrorMessage}, Details: {Details}";
        }
    }
}
