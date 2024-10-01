namespace StateModellingDDD.Domain;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error ?? throw new ArgumentNullException(nameof(error));
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static implicit operator Result(Error error) => new(error);

    public static Result Success() => new();

    public static Result Failure(Error error) => new(error);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value)
    {
        _value = value;
    }

    private Result(Error error)
        : base(error)
    {
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);

    public static Result<T> Success(T value) => new(value);

    public static new Result<T> Failure(Error error) => new(error);
}
