/// <summary>
/// Interface of the class pooling data structure
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPool<T> where T : class
{
    ////////////////////////////////////////////////////////////
    // Public Methods
    /// <summary>
    /// Return an object from the pool (may create a new one if pool is empty).
    /// </summary>
    /// <returns>object from the pool</returns>
    T GetObject();

    /// <summary>
    /// Return an object into the pool for an ulterior use.
    /// </summary>
    /// <param name="pooledObject">the object to return</param>
    void ReleaseObject(T pooledObject);

    /// <summary>
    /// Return all object to the pool.
    /// </summary>
    void ReleaseAllObjects();
}