namespace GymSystem.BLL.Common;


public record Result
{
    public bool IsSuccess { get; init; }

    public string? Message { get; init; }

    public ResultKind Kind { get; init; }

    protected Result()
    {
    }

    public static Result Success()
        => new()
        {
            IsSuccess = true,
            Kind = ResultKind.Ok
        };

    public static Result Failure(
        string message,
        ResultKind kind = ResultKind.Conflict)
        => new()
        {
            IsSuccess = false,
            Message = message,
            Kind = kind
        };

    public static Result Validation(string message)
        => Failure(message, ResultKind.Validation);

    public static Result NotFound(string message)
        => Failure(message, ResultKind.NotFound);
}