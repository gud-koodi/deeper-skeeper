/// <summary>
/// Interface for a generic call wrapper.
/// </summary>
public interface ICallable<T>
{
    /// <summary>
    /// Calls object.
    /// </summary>
    /// <param name="t">any</param>
    void Call(T t);
}
