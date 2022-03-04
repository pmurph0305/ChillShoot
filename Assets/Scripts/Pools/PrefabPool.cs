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
  [SerializeField] bool ResetScaleOnGet = true;
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
        // because if we set GetFromPool here, the Get(position, rotation) methods will call on getfrompool first,
        // whereas one would expect calling Get(position, rotation) would call OnGetFromPool for that item with the
        // position and rotation already set, but it does not.
        // instead we manually handle calling OnGetFromPool.

        // we fixed this by removing the <T>.OnGetFromPool() call after the Get() and settings of position and rotation.
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
    t.OnGetFromPool();
    return t;
  }

  /// <summary>
  /// Gets an item from the pool, while settings its position and rotation.
  /// </summary>
  /// <param name="spawnInfo"></param>
  /// <returns></returns>
  public virtual T Get(TransformSpawnInfo spawnInfo)
  {
    T t = Get();
    t.transform.position = spawnInfo.Position;
    t.transform.rotation = spawnInfo.Rotation;
    t.OnGetFromPool();
    return t;
  }


  /// <summary>
  /// Gets an item from the pool, while settings its position and rotation.
  /// </summary>
  /// <param name="position"></param>
  /// <param name="rotation"></param>
  /// <returns></returns>
  public virtual T Get(Vector3 position, Quaternion rotation)
  {
    T t = Get();
    t.transform.position = position;
    t.transform.rotation = rotation;
    t.OnGetFromPool();
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
    // Since OnDisable releases a pooled item, we can't set active here or it causes issues.
    // IE: if you get multiple of the same object in one frame, it could be released and then used again in the same frame.
    // o.SetActive(false);
    OnCreateInPool?.Invoke(val);
    return val;
  }

  Transform getTransform;

  /// <summary>
  /// Called when an object is gotten to the pool.<br></br>
  /// Calls the IPoolable's OnGetFromPool<br></br>
  /// Sets the object active if required, and Invokes the appropriate action.
  /// </summary>
  /// <param name="item"></param>
  protected virtual void OnGetFromPool(T item)
  {
    getTransform = item.transform;
    if (parentType == ParentType.This)
    {
      if (getTransform.parent != this.transform)
      {
        getTransform.SetParent(this.transform);
      }
    }
    if (ResetScaleOnGet)
    {
      getTransform.localScale = Prefab.transform.localScale;
    }
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
