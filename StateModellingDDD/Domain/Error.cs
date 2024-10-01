namespace StateModellingDDD.Domain;

public sealed record Error(string Message)
{
    public static Error None => new(string.Empty);
}
