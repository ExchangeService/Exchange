namespace Exchange.Shared.Communication
{
    public class OperationEntity<T> : OperationResult
    {
        protected OperationEntity(string? error, string? errorCode, bool succeded, T? id)
            : base(error, errorCode, succeded) =>
            this.OperationResultId = id;

        public T? OperationResultId { get; }

        public static new OperationEntity<T> Error(string? error, string? errorCode = "unknown") => new(error, errorCode, false, default);

        public static OperationEntity<T> Success(T id) => new(null, null, true, id);
    }
}