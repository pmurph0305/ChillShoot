using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;


/// <summary>
/// A generic prefab pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PrefabPool<T> : MonoBehaviour where T : Component, IPoolable<T>
{
  [SerializeField, Tooltip("GameObject.SetActive(true) on pool.Get")] bool SetActiveOnGet = true;

  // Removed because it causes bugs with multiple releases as the the object can be released
  // multiple times between initally telling it to be released, and the object actually being de-activated through the callback.
  // Recommended to use something like releasing in OnDisable to prevent multiple-release issues.
  // [SerializeField, Tooltip("GameObject.SetActive(false) on pool.Release, causes bugs, see IPoolable release comments")] bool SetActiveOnReturned = true;

  /// <summary>
  /// The parent object that pooled objects are created on.
  /// </summary>
  Transform parent;

  public enum ParentType
  {
    None,
    This,
    New
  }
  [SerializeField, Tooltip("Parent of created pooled objects.\nNone:Null parent (Scene Root)\nThis:The transform that this script is on\nNew:Creates a gameobject at the scene root as the parent.")] ParentType parentType;
  [SerializeField] GameObject Prefab;
  IObjectPool<T> _pool;

  [SerializeField] int capacity = 100;


  public event Action<T> OnReturnedToPoolAction;
  public event Action<T> OnGetFromPoolAction;
  public event Action<T> OnCreateInPool;


  private void Awake()
  {
    if (parentType == ParentType.This)
    {
      parent = this.transform;
    }
    else if (parentType == ParentType.New)
    {
      parent = new GameObject(this.gameObject.name + " parent").transform;
    }
    else if (parentType == ParentType.None)
    {
      parent = null;
    }
  }


  protected IObjectPool<T> Pool
  {
    get
    {
      if (_pool == null)
      {
        _pool = new ObjectPool<T>(CreatePooledItem, OnGetFromPool, OnReturnedToPool, OnDestroyPoolObject, false, capacity);
      }
      return _pool;
    }
  }

  /// <summary>
  /// Gets an object from the pool, while setting it's transforms position.
  /// </summary>
  /// <param name="position"></param>
  /// <returns></returns>
  public virtual T Get(Vector3 position)
  {
    T t = Get();
    t.transform.position = position;
    return t;
  }

  /// <summary>
  /// Gets an object from the pool.
  /// </summary>
  /// <returns></returns>
  public virtual T Get()
  {
    return Pool.Get();
  }

  /// <summary>
  /// Creates the pooled items.
  /// </summary>
  /// <returns></returns>
  protected virtual T CreatePooledItem()
  {
    GameObject o = Instantiate(Prefab, transform.position, Quaternion.identity, parent);
    T val = o.GetComponent<T>();
    val.SetPool(Pool);
    val.OnCreate();
    o.SetActive(false);
    OnCreateInPool?.Invoke(val);
    return val;
  }

  /// <summary>
  /// Called when an object is gotten to the pool.<br></br>
  /// Calls the IPoolable's OnGetFromPool<br></br>
  /// Sets the object active if required, and Invokes the appropriate action.
  /// </summary>
  /// <param name="item"></param>
  protected virtual void OnGetFromPool(T item)
  {
    // item.SetPool(Pool);
    item.OnGetFromPool();
    if (SetActiveOnGet)
    {
      item.gameObject.SetActive(true);
    }
    OnGetFromPoolAction?.Invoke(item);
  }

  /// <summary>
  /// Called when an object is returned to the pool.<br></br>
  /// Sets the object inactive if required, and Invokes the appropriate action.
  /// </summary>
  /// <param name="item"></param>
  protected virtual void OnReturnedToPool(T item)
  {
    // if (SetActiveOnReturned)
    // {
    //   item.gameObject.SetActive(false);
    // }
    OnReturnedToPoolAction?.Invoke(item);
  }

  /// <summary>
  /// Called when an object in the pool is destroyed?
  /// </summary>
  /// <param name="item"></param>
  protected void OnDestroyPoolObject(T item)
  {
    Destroy(item.gameObject);
  }
}
