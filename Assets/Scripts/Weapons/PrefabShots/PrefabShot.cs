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
  [SerializeField] Rigidbody2D rb;
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
    enemy.OnEnemyReleased += OnEnemyReleased;
    enemy.OnHitFromShot(weaponInfo.ShotDamage);
    DamagedEnemies.Add(enemy, new Timer(damageCooldown));
  }

  [SerializeField] bool AddPlayerSpeed;
  [SerializeField] bool UseSpeedEaser;
  [SerializeField] FloatEaser speedEase;

  protected float GetSpeed()
  {
    if (UseSpeedEaser)
    {
      return speedEase.current + addedPlayerSpeed;
    }
    return weaponInfo.ShotSpeed + addedPlayerSpeed;
  }

  [SerializeField] float addedPlayerSpeed = 0.0f;
  protected void SetPlayerSpeedParameters()
  {
    // unit vector, instead of using player velocity
    Vector3 playerDirection = PlayerController.PlayerDirection;
    // is a unit vector
    Vector3 intialDir = director.GetTravelDirection();
    // amount of player speed in the same direction of the travel direction.
    float dot = Vector3.Dot(playerDirection, intialDir); // both unit vectors.
    addedPlayerSpeed = dot * PlayerController.PlayerSpeed;
    // }
  }

  /// <summary>
  /// Updates the travel direction through GetTravelDirection(), and uses it to update the position of the gameobject.
  /// </summary>
  protected virtual void UpdateMovement(bool fixedDeltaTime = false)
  {
    // so using the rigidbody, we have to do this, but it causes an issue if the spawn location isn't the initial
    // position for an offset shot.
    // update does not have the issue, but then needs to sync transforms.
    Vector3 delta = director.GetScaledMovement(GetSpeed(), fixedDeltaTime ? Time.fixedDeltaTime : Time.deltaTime);
    if (fixedDeltaTime)
    {
      rb.MovePosition(rb.position + (Vector2)delta);
    }
    else
    {
      transform.position += delta;
    }
  }

  /// <summary>
  /// Called when the object is gotten from the object pool. Use like you would OnEnable.<br></br>
  /// Useful for setting a specific target(s) or travel direction when the object is "Shot"
  /// </summary>
  public virtual void OnGetFromPool()
  {
    director.OnGetFromPool();
    rb.MovePosition(director.GetInitialPosition());
    Physics2D.SyncTransforms();
    DamagedEnemies.Clear();
    lifeTimer.Reset(weaponInfo.ShotLifeTime);
    NumberOfHits = 0;
    OnGetFromPoolAction?.Invoke(this);
    if (UseSpeedEaser)
    {
      speedEase.StartEase(weaponInfo.ShotSpeed);
    }
    if (AddPlayerSpeed)
    {
      SetPlayerSpeedParameters();
    }
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
    if (UseSpeedEaser)
    {
      speedEase.Stop();
    }
  }


  // Update is called once per frame
  // void Update()
  // {
  //   UpdateMovement(false);
  //   if (lifeTimer.Update(Time.deltaTime))
  //   {
  //     Release();
  //   }
  //   OnUpdate(Time.deltaTime);
  // }

  // causes issues because on spawn
  // if we call SyncTransform manually, with the new get initial offset method, it works.
  void FixedUpdate()
  {
    UpdateMovement(true);
    if (lifeTimer.Update(Time.fixedDeltaTime))
    {
      Release();
    }
    OnUpdate(Time.fixedDeltaTime);
  }


  CircleCollider2D circle;
  Collider2D[] hitColliders;
  [SerializeField] LayerMask mask;

  [SerializeField] int DamagedEnemiesCount;
  /// <summary>
  /// Called At the end of Update()
  /// </summary>
  protected virtual void OnUpdate(float deltaTime)
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
    foreach (var item in EnteredEnemies)
    {
      if (!DamagedEnemies.ContainsKey(item))
      {
        OnHitEnemy(item);
      }
    }
    foreach (var item in ExitedEnemies)
    {
      DamagedEnemies.Remove(item);
    }
    foreach (var item in ReleasedEnemies)
    {
      DamagedEnemies.Remove(item);
    }
    foreach (var kvp in DamagedEnemies)
    {
      if (kvp.Value.Update(deltaTime))
      {
        kvp.Value.Reset();
        kvp.Key.OnHitFromShot(weaponInfo.ShotDamage);
      }
    }
    EnteredEnemies.Clear();
    ExitedEnemies.Clear();
    ReleasedEnemies.Clear();
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


  HashSet<EnemyController> EnteredEnemies = new HashSet<EnemyController>();

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (EnemyDictionary.ContainsActive(other))
    {
      EnemyController ec = EnemyDictionary.GetActive(other);
      EnteredEnemies.Add(ec);
    }
  }

  List<EnemyController> ReleasedEnemies = new List<EnemyController>();
  void OnEnemyReleased(EnemyController ec)
  {
    ec.OnEnemyReleased -= OnEnemyReleased;
    ReleasedEnemies.Add(ec);
    // Debug.Log("Released.");
  }

  HashSet<EnemyController> ExitedEnemies = new HashSet<EnemyController>();
  private void OnTriggerExit2D(Collider2D other)
  {
    if (EnemyDictionary.Contains(other))
    {
      // Debug.Log("Remove on exit.");
      EnemyController ec = EnemyDictionary.Get(other);
      // DamagedEnemies.Remove(ec);
      ExitedEnemies.Add(ec);
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
