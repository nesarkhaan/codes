namespace FileShare.Web.Models;
// Represents the result of an operation, indicating whether it succeeded and providing a message.
public readonly record struct OperationResult(bool Succeeded, string Message)
{
    // Creates a successful operation result with the specified message.
    public static OperationResult Success(string message) => new(true, message);
    // Creates a failed operation result with the specified message.
    public static OperationResult Failure(string message) => new(false, message);
}
