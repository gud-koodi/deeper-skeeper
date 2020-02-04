/// <summary>
/// Interface for a generic call wrapper.
/// </summary>
public interface ICallable<T>
{
    /// <summary>
    /// Calls object.
    /// </summary>
    /// <typeparam name="t">Generic type.</typeparam>
    void Call(T t);
}
