/// <summary>
/// Interface for a generic call wrapper.
/// </summary>
/// /// <typeparam name="T">Generic type.</typeparam>
public interface ICallable<T>
{
    /// <summary>
    /// Calls object.
    /// </summary>
    /// <typeparam name="t">Generic type.</typeparam>
    /// <param name="t">What to call.</param>
    void Call(T t);
}
