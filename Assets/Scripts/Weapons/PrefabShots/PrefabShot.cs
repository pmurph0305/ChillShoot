using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;


//TODO: A separate component that just returns the travel direction, so we can use different combinations and mix and match.

/// <summary>
/// An abstract class so we can create different types of Prefab Shots and place those onto items but retain the same pool and dictionary functionality.
/// Responsible for tracking its own life-time, updating its movement, and getting its travel direction for updating movement.
/// Also responsible for disabling/releasing to the pool when it hits an enemy (OnHitEnemy is called from the WeaponInfo class)
/// </summary>
public class PrefabShot : MonoBehaviour, IPoolable<PrefabShot>, IWeaponShot, ITargetProvider
{
  public WeaponInfo weaponInfo { get; protected set; }

  [SerializeField] Collider2D col;
  [SerializeField] protected Timer lifeTimer;

  [SerializeField] TravelDirector director;

  public IObjectPool<PrefabShot> pool;

  //todo: number of hits before destroy/release
  [SerializeField] bool DestroyOnHit = true;
  [SerializeField] int DestroyAfterXHits = 0;
  [SerializeField] float damageCooldown = 0.25f;

  Dictionary<EnemyController, Timer> DamagedEnemies = new Dictionary<EnemyController, Timer>();

  int NumberOfHits = 0;

  public event Action<PrefabShot> OnCreateAction;
  public event Action<PrefabShot> OnGetFromPoolAction;
  public event Action<PrefabShot> OnReleaseAction;

  /// <summary>
  /// Called by the weapon info of this shot when an enemy detects that this shot hit the enemy.
  /// </summary>
  public virtual void OnHitEnemy(EnemyController enemy)
  {
    if (DestroyOnHit)
    {
      NumberOfHits++;
      if (NumberOfHits >= DestroyAfterXHits)
      {
        Release();
      }
    }
  }

  /// <summary>
  /// Gets the travel direction of the shot
  /// </summary>
  /// <returns></returns>
  protected virtual Vector3 GetTravelDirection()
  {
    return director.GetTravelDirection();
  }

  /// <summary>
  /// Updates the travel direction through GetTravelDirection(), and uses it to update the position of the gameobject.
  /// </summary>
  protected virtual void UpdateMovement()
  {
    // transform.position += Time.deltaTime * weaponInfo.ShotSpeed * GetTravelDirection();
    director.UpdateMovement(weaponInfo.ShotSpeed);
  }

  /// <summary>
  /// Called when the object is gotten from the object pool. Use like you would OnEnable.<br></br>
  /// Useful for setting a specific target(s) or travel direction when the object is "Shot"
  /// </summary>
  public virtual void OnGetFromPool()
  {
    DamagedEnemies.Clear();
    lifeTimer.Reset(weaponInfo.ShotLifeTime);
    NumberOfHits = 0;
    OnGetFromPoolAction?.Invoke(this);
    director.OnGetFromPool();
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
    OnReleaseAction?.Invoke(this);
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


  CircleCollider2D circle;
  Collider2D[] hitColliders;
  [SerializeField] LayerMask mask;

  [SerializeField] int DamagedEnemiesCount;
  /// <summary>
  /// Called At the end of Update()
  /// </summary>
  protected virtual void OnUpdate()
  {
    DamagedEnemiesCount = DamagedEnemies.Count;
    if (DestroyOnHit && DestroyAfterXHits == 0 && DamagedEnemiesCount > 0)
    {
      Debug.LogWarning("Hit issue?", this.gameObject);
      foreach (var kvp in DamagedEnemies)
      {
        Debug.LogWarning("Still damaging:", kvp.Key.gameObject);
      }
    }
    foreach (var kvp in DamagedEnemies)
    {
      if (kvp.Value.Update())
      {
        kvp.Value.Reset();
        kvp.Key.OnHitFromShot(weaponInfo.ShotDamage);
      }
    }
    foreach (var item in ReleasedEnemies)
    {
      DamagedEnemies.Remove(item);
    }
    ReleasedEnemies.Clear();
    // if (circle == null)
    // {
    //   if (col is CircleCollider2D)
    //   {
    //     circle = (CircleCollider2D)col;
    //   }
    // }
    // hitColliders = Physics2D.OverlapCircleAll(transform.position, circle.radius * transform.localScale.x, mask);
    // foreach (var h in hitColliders)
    // {
    //   if (EnemyDictionary.Contains(h))
    //   {
    //     EnemyController ec = EnemyDictionary.Get(h);
    //     ec.OnHitFromShot(weaponInfo.ShotDamage);
    //     NumberOfHits++;
    //     if (DestroyOnHit)
    //     {
    //       if (NumberOfHits >= DestroyAfterXHits)
    //       {
    //         Release();
    //         break;
    //       }
    //     }
    //   }
    // }
  }

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
  public virtual void OnCreate()
  {
    weaponInfo = WeaponDictionary.Get(this.gameObject.tag);
    lifeTimer = new Timer(weaponInfo.ShotLifeTime);

    //Register this created gameobject to the weapon info's dictionary.
    weaponInfo.Add(this, col);
    OnCreateAction?.Invoke(this);
  }


  private void OnTriggerEnter2D(Collider2D other)
  {
    if (EnemyDictionary.ContainsActive(other))
    {
      EnemyController ec = EnemyDictionary.GetActive(other);
      ec.OnHitFromShot(weaponInfo.ShotDamage);
      OnHitEnemy(ec);
      DamagedEnemies.Add(ec, new Timer(damageCooldown));
      ec.OnEnemyReleased += OnEnemyReleased;
    }
  }

  List<EnemyController> ReleasedEnemies = new List<EnemyController>();
  void OnEnemyReleased(EnemyController ec)
  {
    ec.OnEnemyReleased -= OnEnemyReleased;
    ReleasedEnemies.Add(ec);
    // Debug.Log("Released.");
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (EnemyDictionary.Contains(other))
    {
      // Debug.Log("Remove on exit.");
      EnemyController ec = EnemyDictionary.Get(other);
      DamagedEnemies.Remove(ec);
    }
  }

  public Transform GetTarget()
  {
    if (weaponInfo is TargetedWeaponInfo)
    {
      return ((TargetedWeaponInfo)weaponInfo).target;
    }
    return null;
  }
}
