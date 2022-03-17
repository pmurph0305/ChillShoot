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
  [SerializeField] WeaponKey weaponKey;
  public WeaponInfo weaponInfo { get; protected set; }
  [SerializeField] Collider2D col;
  [SerializeField] Rigidbody2D rb;
  [SerializeField] protected Timer lifeTimer;
  [SerializeField] TravelDirector director;
  protected IObjectPool<PrefabShot> pool;
  Dictionary<EnemyController, Timer> DamagedEnemies = new Dictionary<EnemyController, Timer>();
  protected int NumberOfHits = 0;

  public event Action OnGetFromPoolAction;

  /// <summary>
  /// Called by the weapon info of this shot when an enemy detects that this shot hit the enemy.
  /// </summary>
  public virtual void OnHitEnemy(EnemyController enemy)
  {
    Debug.Log("Hit", enemy);
    if (weaponInfo.DestroyOnHit)
    {
      NumberOfHits++;
      if (NumberOfHits >= weaponInfo.DestroyAfterXHits)
      {
        Release();
      }
    }
    EffectPlayerPool.StartEffect(transform.position);
    if (!DamagedEnemies.ContainsKey(enemy))
    {
      enemy.OnEnemyReleased += OnEnemyReleased;
      DamagedEnemies.Add(enemy, new Timer(weaponInfo.DamageCooldown));
    }
    enemy.OnHitFromShot(weaponInfo.ShotDamage);
  }

  [SerializeField] bool UseSpeedEaser;
  [SerializeField] FloatEaser speedEase;

  protected float GetSpeed()
  {
    if (UseSpeedEaser)
    {
      return speedEase.current;
    }
    return weaponInfo.ShotSpeed;
  }


  [SerializeField] Vector3 playerVelocity;
  protected void SetPlayerSpeedParameters()
  {
    playerVelocity = PlayerController.PlayerVelocity;
    if (weaponInfo.AddPlayerSpeed)
    {
      director.SetAdditionalVelocity(playerVelocity);
    }
    else
    {
      director.SetAdditionalVelocity(Vector3.zero);
    }
  }


  /// <summary>
  /// Updates the travel direction through GetTravelDirection(), and uses it to update the position of the gameobject.
  /// </summary>
  protected virtual void UpdateMovement(bool fixedDeltaTime = false)
  {
    if (fixedDeltaTime)
    {
      director.UpdateMovement(rb, GetSpeed(), Time.fixedDeltaTime);
    }
    else
    {
      director.UpdateMovement(GetSpeed(), Time.deltaTime);
      // since it only updates on the next fixed update, this doesn't actually preserve the speed correctly.
      // director.UpdateMovement(rb, GetSpeed(), Time.deltaTime);
    }
  }

  /// <summary>
  /// Called when the object is gotten from the object pool. Use like you would OnEnable.<br></br>
  /// Useful for setting a specific target(s) or travel direction when the object is "Shot"
  /// </summary>
  public virtual void OnGetFromPool()
  {
    Vector3 s = transform.localScale;
    director.OnGetFromPool();
    // rb.MovePosition(director.GetInitialPosition());
    // Physics2D.SyncTransforms();
    DamagedEnemies.Clear();
    shotTweener.OnGetFromPool();
    lifeTimer.Reset(weaponInfo.GetLifeTime() - shotTweener.GetTweenOutDuration());
    NumberOfHits = 0;
    // OnGetFromPoolAction?.Invoke(this);
    // transform.localScale = Vector3.one * weaponInfo.ScaleMultiplier;
    if (UseSpeedEaser)
    {
      speedEase.StartEase(weaponInfo.ShotSpeed);
    }
    SetPlayerSpeedParameters();

    shotTweener.StartTweenIn(this.transform, Vector3.one * weaponInfo.GetScaleMultiplier(), () => { });
    OnGetFromPoolAction?.Invoke();
    // Debug.Log(s + " : " + transform.localScale, this.transform);
  }


  [SerializeField] ShotTweener shotTweener;
  /// <summary>
  /// Called when the object is about to be released to the pool. Use like you would SetActive(false).
  /// Doesn't actually release the shot, instead that is done when it is actually disabled, otherwise you could re-use it before it is disabled,
  /// and then it would be disabled and cause pooling errors.
  /// </summary>
  public void Release()
  {
    // if (shotTweener.TweenOut)
    // {
    shotTweener.StartTweenOut(this.transform, () => this.gameObject.SetActive(false));
    // }
    // else
    // {
    //   this.gameObject.SetActive(false);
    // }
  }

  private void OnDisable()
  {
    OnDisabled();
  }

  protected virtual void OnDisabled()
  {
    // released in on disable to prevent pool issues, see IPoolable comments
    if (UseSpeedEaser)
    {
      speedEase.Stop();
    }
    // if (!lifeTimer.IsFinished && (!weaponInfo.DestroyOnHit || (weaponInfo.DestroyOnHit && NumberOfHits < weaponInfo.DestroyAfterXHits)))
    // {
    //   Debug.LogWarning($"{this.transform.name} Timer:{lifeTimer.IsFinished}, DOH:{weaponInfo.DestroyOnHit} Num Hits:{NumberOfHits}, X:{weaponInfo.DestroyAfterXHits}", this.transform);
    // }
    pool.Release(this);
  }


  // Update is called once per frame
  void Update()
  {
    UpdateMovement(false);
    OnUpdate(Time.deltaTime);
  }



  // causes issues because on spawn
  // // if we call SyncTransform manually, with the new get initial offset method, it works.
  // void FixedUpdate()
  // {
  //   // UpdateMovement(true);
  //   // if (!lifeTimer.IsFinished && lifeTimer.Update(Time.fixedDeltaTime))
  //   // {
  //   //   Release();
  //   // }
  //   // OnUpdate(Time.fixedDeltaTime);
  // }

  [SerializeField] int DamagedEnemiesCount;
  /// <summary>
  /// Called At the end of Update()
  /// </summary>
  protected virtual void OnUpdate(float deltaTime)
  {
    if (!lifeTimer.IsFinished && lifeTimer.Update(deltaTime))
    {
      Release();
    }
    DamagedEnemiesCount = DamagedEnemies.Count;
    if (weaponInfo.DestroyOnHit && weaponInfo.DestroyAfterXHits == 0 && DamagedEnemiesCount > 0)
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
        // kvp.Key.OnHitFromShot(weaponInfo.ShotDamage);
        OnHitEnemy(kvp.Key);
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
    weaponInfo = WeaponDictionary.Get(weaponKey);
    lifeTimer = new Timer(weaponInfo.GetLifeTime() - shotTweener.GetTweenOutDuration());
    //Register this created gameobject to the weapon info's dictionary.
    weaponInfo.Add(this, col);
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
