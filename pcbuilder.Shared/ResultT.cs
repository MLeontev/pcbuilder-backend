using System.Diagnostics.CodeAnalysis;

namespace pcbuilder.Shared;

public class Result<T> : Result
{
    private readonly T? _value;

    public Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Failure result");
}