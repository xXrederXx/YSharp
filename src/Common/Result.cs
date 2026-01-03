using System.Diagnostics.CodeAnalysis;

namespace YSharp.Common;

/// <summary>
/// A class that represents the result of an operation. If it succeded it carries a non-null value.
/// If it failed it will carry a non-null error. The class provides functions to generate succesfull and failed results.
/// </summary>
/// <typeparam name="TValue">This is the type the success value has</typeparam>
/// <typeparam name="TError">This is the type of the error</typeparam>
public readonly struct Result<TValue, TError> : IEquatable<Result<TValue, TError>>
{
    private readonly TValue? _Value;
    private readonly TError? _Error;

    public bool IsSuccess { get; }
    public bool IsFailed => !IsSuccess;

    private Result(TValue value)
    {
        _Value = value;
        _Error = default;
        IsSuccess = true;
    }

    private Result(TError error)
    {
        _Value = default;
        _Error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// This function generates and returns a new result which represents a successful operation.
    /// </summary>
    /// <typeparam name="T">Type for the success value</typeparam>
    /// <typeparam name="U">Type for the not existing error</typeparam>
    /// <param name="value">The value produced and needed to give along</param>
    /// <returns>A new instance of <c>Result</c> with a value of type <c>T</c> and IsSucess set to true </returns>
    /// <exception cref="ArgumentNullException">The value is not allowed to be null. In this case the function throws</exception>
    public static Result<TValue, TError> Succses(TValue value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
        return new Result<TValue, TError>(value);
    }

    /// <summary>
    /// This function generates and returns a new result which represents a failed operation.
    /// </summary>
    /// <typeparam name="T">Type for the non existing success value</typeparam>
    /// <typeparam name="U">Type for the error</typeparam>
    /// <param name="error">The error which occured durring the operations</param>
    /// <returns>A new instance of <c>Result</c> with a error of type <c>U</c> and IsSucess set to false </returns>
    /// <exception cref="ArgumentNullException">The error is not allowed to be null. In this case the function throws</exception>
    public static Result<TValue, TError> Fail(TError error)
    {
        if (error is null)
            throw new ArgumentNullException(nameof(error));
        return new Result<TValue, TError>(error);
    }

    /// <summary>
    /// This function can be used to try to get a value. Only use the value if the function returns true
    /// </summary>
    /// <param name="value">This will be the value saved or the null if IsSuccess is false</param>
    /// <returns>True if the result was a success and false otherwise</returns>
    public readonly bool TryGetValue([NotNullWhen(true)] out TValue value)
    {
        value = _Value!;
        return IsSuccess;
    }

    /// <summary>
    /// This returns the saved value. Use with caution. Use TryGetValue if possible.
    /// </summary>
    /// <returns>The value from the result</returns>
    /// <exception cref="InvalidOperationException">If the result was not successful it will throw</exception>
    public readonly TValue GetValue()
    {
        if (IsFailed)
            throw new InvalidOperationException(
                "You cant get the value of a failed Result. Please use tryGet or check state first"
            );
        return _Value!;
    }

    /// <summary>
    /// This returns the saved error. Use with caution. Only use if the result is failed
    /// </summary>
    /// <returns>The error from the result</returns>
    /// <exception cref="InvalidOperationException">If the result was successful it will throw</exception>
    public readonly TError GetError()
    {
        if (IsSuccess)
            throw new InvalidOperationException(
                "You cant get the error of a succeded Result. Please check state first"
            );
        return _Error!;
    }

    public readonly void Deconstruct(out bool isSuccess, out TValue? value, out TError? error)
    {
        isSuccess = IsSuccess;
        value = _Value;
        error = _Error;
    }

    public override readonly int GetHashCode()
    {
        return IsSuccess ? HashCode.Combine(true, _Value) : HashCode.Combine(false, _Error);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is not Result<TValue, TError> other)
            return false;
        return Equals(other);
    }

    public readonly bool Equals(Result<TValue, TError> other)
    {
        if (IsSuccess != other.IsSuccess)
            return false;

        return IsSuccess
            ? EqualityComparer<TValue>.Default.Equals(_Value, other._Value)
            : EqualityComparer<TError>.Default.Equals(_Error, other._Error);
    }

    public static bool operator ==(Result<TValue, TError> left, Result<TValue, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TValue, TError> left, Result<TValue, TError> right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return IsSuccess ? $"Success({_Value})" : $"Fail({_Error})";
    }
}
