
namespace GymSystem.BLL.Common;
public sealed record Result<T> : Result
{
    public T? Value { get; init; }

    private Result()
    {
    }

    public static Result<T> Success(T value)
        => new()
        {
            IsSuccess = true,
            Value = value,
            Kind = ResultKind.Ok
        };

    public new static Result<T> Failure(
        string message,
        ResultKind kind = ResultKind.Conflict)
        => new()
        {
            IsSuccess = false,
            Message = message,
            Kind = kind
        };

    public new static Result<T> Validation(string message)
        => Failure(message, ResultKind.Validation);

    public new static Result<T> NotFound(string message)
        => Failure(message, ResultKind.NotFound);
}