using System.Collections.Generic;
using UnityEngine;

public class ObjectPools<T> : IPool<T> where T : MonoBehaviour
{
    #region DECLARATIONS
    /// <summary>
    ///prefab reference for instantiating new objects
    /// </summary>
    private GameObject prefab;
    /// <summary>
    /// parent reference under where active prefabs will be present
    /// </summary>
    private Transform objectParent;
    /// <summary>
    /// parent reference under which deactivated prefabs will be present
    /// </summary>
    private Transform releasedParent;

    /// <summary>
    /// list of all active objects
    /// </summary>
    private List<T> activeObjects = new List<T>();
    /// <summary>
    /// list of all deactive objects
    /// </summary>
    private List<T> inactiveObjects = new List<T>();
    #endregion

    /// <summary>
    /// Gets the active objects count.
    /// </summary>
    /// <value>Total active objects</value>
    public int ActiveObjectCount
    {
        get
        {
            return activeObjects.Count;
        }
    }

    /// <summary>
    /// Gets the inactive objects count.
    /// </summary>
    /// <value>Total inactive objects</value>
    public int InactiveObjectCount
    {
        get
        {
            return inactiveObjects.Count;
        }
    }

    /// <summary>
    /// Initializes a new Instance of this class
    /// </summary>
    /// <param name="prefab">Prefab reference for instantiating objects</param>
    /// <param name="objectParent">Active prefabs parent.</param>
    /// <param name="releasedParent">Released/deactive prefabs parent.</param>
    public ObjectPools(GameObject prefab, Transform objectParent = null, Transform releasedParent = null)
    {
        this.prefab = prefab;
        this.objectParent = objectParent;
        this.releasedParent = releasedParent;
    }

    #region PUBLIC METHODS
    /// <summary>
    /// Finds the active object given predicate condition
    /// </summary>
    /// <returns>active object.</returns>
    /// <param name="condition">Condition.</param>
    public T FindActiveObject(System.Predicate<T> condition)
    {
        return activeObjects.Find(t => condition.Invoke(t));
    }

    /// <summary>
    /// Finds the inactive object given predicate condition
    /// </summary>
    /// <returns>Inactive object.</returns>
    /// <param name="condition">Condition.</param>
    public T FindInactiveObject(System.Predicate<T> condition)
    {
        return inactiveObjects.Find(t => condition.Invoke(t));
    }

    #region IPool implementation

    /// <summary>
    /// Return an object from the pool (may create a new one if pool is empty).
    /// </summary>
    /// <returns>object from the pool</returns>
    public T GetObject()
    {
        T newObject = null;
        if (inactiveObjects.Count > 0)
        {
            newObject = inactiveObjects[0];
            inactiveObjects.RemoveAt(0);
        }
        else
        {
            GameObject newGo = MonoBehaviour.Instantiate(prefab);
            newObject = newGo.GetComponent<T>();
        }
        activeObjects.Add(newObject);
        if (objectParent != null)
        {
            newObject.transform.SetParent(objectParent, false);
        }
        return newObject;
    }

    /// <summary>
    /// Return an object into the pool for an ulterior use.
    /// </summary>
    /// <param name="pooledObject">the object to return</param>
    public void ReleaseObject(T pooledObject)
    {
        if (!inactiveObjects.Contains(pooledObject))
        {
            inactiveObjects.Add(pooledObject);
            activeObjects.Remove(pooledObject);
            if (releasedParent != null)
            {
                pooledObject.transform.SetParent(releasedParent, false);
            }
        }
    }

    /// <summary>
    /// Return all object to the pool.
    /// </summary>
    public void ReleaseAllObjects()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            if (releasedParent != null)
            {
                activeObjects[i].transform.SetParent(releasedParent, false);
            }
            inactiveObjects.Add(activeObjects[i]);
            activeObjects.RemoveAt(i);
            i--;
        }
    }
    #endregion
    #endregion
}