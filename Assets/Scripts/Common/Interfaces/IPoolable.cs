using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public interface IPoolable<T> where T : Component
{
  //Pools do not need to always Activate/Deactivate gameobjects automatically
  // so we can't always use OnEnable/Disable.

  /// <summary>
  /// Called by the pool when the object is gotten from the pool.<br></br>
  /// Use this when the object needs to be re-initialized like in an OnEnable
  /// </summary>
  void OnGetFromPool();

  /// <summary>
  /// Called by the pool to set the object's pool, so it can return itself to the pool.
  /// </summary>
  /// <param name="pool"></param>
  void SetPool(IObjectPool<T> pool);

  /// <summary>
  /// Called by the pool when the object is created.<br></br>
  /// Use it similar to how you would use Awake/Start
  /// </summary>
  void OnCreate();

  /// <summary>
  /// Method that begins whatever releases the object to the pool.
  /// Generally a call to gameobject.SetActive
  /// doing pool.Release() in this method can cause issues with multiple releases of the same object
  /// if its not managed elsewhere, but using setactive and releasing in ondisable, the object is only released once.
  /// </summary>
  void Release();

}
