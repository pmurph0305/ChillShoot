using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// An abstract class so we can create different types of Prefab Shots and place those onto items but retain the same pool and dictionary functionality.
/// Responsible for tracking its own life-time, updating its movement, and getting its travel direction for updating movement.
/// Also responsible for disabling/releasing to the pool when it hits an enemy (OnHitEnemy is called from the WeaponInfo class)
/// </summary>
public abstract class PrefabShot : MonoBehaviour, IPoolable<PrefabShot>, IWeaponShot
{
  protected WeaponInfo weaponInfo;
  [SerializeField] Collider2D col;
  [SerializeField] protected Vector3 travelDirection;
  [SerializeField] protected Timer lifeTimer;

  public IObjectPool<PrefabShot> pool;

  /// <summary>
  /// Called by the weapon info of this shot when an enemy detects that this shot hit the enemy.
  /// </summary>
  public virtual void OnHitEnemy()
  {
    Release();
  }

  /// <summary>
  /// Gets the travel direction of the shot
  /// </summary>
  /// <returns></returns>
  protected virtual Vector3 GetTravelDirection()
  {
    return travelDirection;
  }

  /// <summary>
  /// Updates the travel direction through GetTravelDirection(), and uses it to update the position of the gameobject.
  /// </summary>
  protected virtual void UpdateMovement()
  {
    travelDirection = GetTravelDirection();
    transform.position += Time.deltaTime * weaponInfo.ShotSpeed * travelDirection;
  }

  /// <summary>
  /// Called when the object is gotten from the object pool. Use like you would OnEnable.<br></br>
  /// Useful for setting a specific target(s) or travel direction when the object is "Shot"
  /// </summary>
  public virtual void OnGetFromPool()
  {
    lifeTimer.Reset();
  }

  /// <summary>
  /// Called when the object is about to be released to the pool. Use like you would SetActive(false).
  /// Doesn't actually release the shot, instead that is done when it is actually disabled, otherwise you could re-use it before it is disabled,
  /// and then it would be disabled and cause pooling errors.
  /// </summary>
  public void Release()
  {
    this.gameObject.SetActive(false);
  }

  private void OnDisable()
  {
    OnDisabled();
  }

  protected virtual void OnDisabled()
  {
    // released in on disable to prevent pool issues, see IPoolable comments
    pool.Release(this);
  }

  // Update is called once per frame
  void Update()
  {
    UpdateMovement();
    if (lifeTimer.Update())
    {
      Release();
    }
    OnUpdate();
  }

  /// <summary>
  /// Called At the end of Update()
  /// </summary>
  protected virtual void OnUpdate() { }

  /// <summary>
  /// Sets the objects pool so it can handle releasing itself.
  /// </summary>
  /// <param name="pool"></param>
  public void SetPool(IObjectPool<PrefabShot> pool)
  {
    this.pool = pool;
  }


  /// <summary>
  /// Called by the pool when this object is created, use for initialization like you would Start/Awake
  /// </summary>
  public void OnCreate()
  {
    weaponInfo = WeaponDictionary.Get(this.gameObject.tag);
    lifeTimer = new Timer(weaponInfo.ShotLifeTime);

    //Register this created gameobject to the weapon info's dictionary.
    weaponInfo.Add(this, col);
  }
}
