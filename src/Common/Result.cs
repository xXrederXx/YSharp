namespace YSharp.Common;

/// <summary>
/// A class that represents the result of an operation. If it succeded it carries a non-null value.
/// If it failed it will carry a non-null error. The class provides functions to generate succesfull and failed results.
/// </summary>
/// <typeparam name="TValue">This is the type the success value has</typeparam>
/// <typeparam name="TError">This is the type of the error</typeparam>
public class Result<TValue, TError>
{
    private readonly TValue? _Value;
    private readonly TError? _Error;
    private readonly bool _IsSuccess;

    public bool IsSuccess => _IsSuccess;
    public bool IsFailed => !_IsSuccess;

    protected Result(TValue? value, TError? error, bool isSuccess)
    {
        _Value = value;
        _Error = error;
        _IsSuccess = isSuccess;
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
            throw new ArgumentNullException(nameof(value), "The value can not be null");
        return new Result<TValue, TError>(value, default, true);
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
            throw new ArgumentNullException(nameof(error), "The error can not be null");
        return new Result<TValue, TError>(default, error, false);
    }

    /// <summary>
    /// This function can be used to try to get a value.
    /// </summary>
    /// <param name="value">This will be the value saved or the defaultValue provided if IsSuccess is false</param>
    /// <param name="defaultValue">The defaultValue if IsSuccess is false</param>
    /// <returns>True if the result was a success and false otherwise</returns>
    public bool TryGetValue(out TValue value, TValue defaultValue)
    {
        value = _IsSuccess ? _Value! : defaultValue;
        return _IsSuccess;
    }

    /// <summary>
    /// This returns the saved value. Use with caution. Use TryGetValue if possible.
    /// </summary>
    /// <returns>The value from the result</returns>
    /// <exception cref="InvalidOperationException">If the result was not successfull it will throw</exception>
    public TValue GetValue()
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
    /// <exception cref="InvalidOperationException">If the result was successfull it will throw</exception>
    public TError GetError()
    {
        if (IsSuccess)
            throw new InvalidOperationException(
                "You cant get the error of a succeded Result. Please check state first"
            );
        return _Error!;
    }
}
