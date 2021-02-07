namespace Exchange.Shared.Communication
{
    public class OperationResult
    {
        protected OperationResult(string? errorMessage, string? errorCode, bool succeded)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorCode = errorCode;
            this.Succeded = succeded;
        }

        public string? ErrorCode { get; }

        public string? ErrorMessage { get; }

        public bool Succeded { get; }

        public static OperationResult Error(string? error, string? errorCode = "unknown") => new(error, errorCode, false);

        public static OperationResult Success() => new(null, null, true);
    }
}