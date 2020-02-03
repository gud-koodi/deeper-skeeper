/// <summary>
/// Interface for a generic call wrapper.
/// </summary>
public interface ICallable<T>
{
    /// <summary>
    /// Calls object.
    /// </summary>
    void Call(T t);
}
