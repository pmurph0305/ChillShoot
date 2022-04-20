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
public abstract class PrefabShotBase : MonoBehaviour, IPoolable<PrefabShotBase>, IWeaponShot, ITargetProvider
{
  [Header("Common")]
  [SerializeField] WeaponKey weaponKey;
  public WeaponInfo weaponInfo { get; protected set; }
  [SerializeField] protected ITravelDirector director;

  [SerializeField] bool UseSpeedEaser;
  [SerializeField] FloatEaser speedEase;
  [SerializeField] ShotTweener shotTweener;

  [Header("Debug")]
  [SerializeField] protected Timer lifeTimer;
  [SerializeField] Vector3 playerVelocity;

  protected IObjectPool<PrefabShotBase> pool;
  Dictionary<IShotDamageable, Timer> DamagedEnemies = new Dictionary<IShotDamageable, Timer>();
  [SerializeField] int DamagedEnemiesCount;
  protected HashSet<IShotDamageable> EnteredEnemies = new HashSet<IShotDamageable>();
  protected HashSet<IShotDamageable> ExitedEnemies = new HashSet<IShotDamageable>();
  List<IShotDamageable> ReleasedEnemies = new List<IShotDamageable>();

  protected int NumberOfHits = 0;


  public event Action OnGetFromPoolAction;

  /// <summary>
  /// When a bullet hits an enemy and damages it, this event is raised.
  /// </summary>
  public static event Action<ShotHitEventArgs> OnHitEnemyAction;

  /// <summary>
  /// Called by the weapon info of this shot when an enemy detects that this shot hit the enemy.
  /// </summary>
  public virtual void OnHitEnemy(IShotDamageable enemy)
  {
    if (enemy.isActiveAndEnabled())
    {
      if (weaponInfo.DestroyOnHit)
      {
        NumberOfHits++;
        if (NumberOfHits >= weaponInfo.DestroyAfterXHits)
        {
          Release();
        }
      }
      if (!DamagedEnemies.ContainsKey(enemy))
      {
        enemy.OnEnemyReleased += OnEnemyReleased;
        DamagedEnemies.Add(enemy, new Timer(weaponInfo.DamageCooldown));
      }

      OnHitEnemyAction?.Invoke(new ShotHitEventArgs(weaponKey, transform.position, enemy.GetTransform().position, director.GetTravelDirection(), enemy.GetTransform(), weaponInfo.GetShotDamage()));
      enemy.OnHitFromShot(this);
    }
  }

  protected float GetSpeed()
  {
    if (UseSpeedEaser)
    {
      return speedEase.current;
    }
    return weaponInfo.GetShotSpeed();
  }

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

  protected abstract void UpdateMovement();
  protected abstract void UpdateFixedMovement();


  /// <summary>
  /// Called when the object is gotten from the object pool. Use like you would OnEnable.<br></br>
  /// Useful for setting a specific target(s) or travel direction when the object is "Shot"
  /// </summary>
  public virtual void OnGetFromPool()
  {
    Vector3 s = transform.localScale;
    director.ResetTravelDirector();
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
      speedEase.StartEase(weaponInfo.GetShotSpeed());
    }
    SetPlayerSpeedParameters();

    shotTweener.StartTweenIn(this.transform, Vector3.one * weaponInfo.GetScaleMultiplier(), () => { });
    OnGetFromPoolAction?.Invoke();
    // Debug.Log(s + " : " + transform.localScale, this.transform);
  }



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
    UpdateMovement();
    OnUpdate(Time.deltaTime);
  }

  private void FixedUpdate()
  {
    UpdateFixedMovement();
  }


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
  public void SetPool(IObjectPool<PrefabShotBase> pool)
  {
    this.pool = pool;
  }


  /// <summary>
  /// Called by the pool when this object is created, use for initialization like you would Start/Awake
  /// </summary>
  public virtual void OnCreate()
  {
    director = GetComponent<ITravelDirector>();
    weaponInfo = WeaponDictionary.Get(weaponKey);
    lifeTimer = new Timer(weaponInfo.GetLifeTime() - shotTweener.GetTweenOutDuration());
    //Register this created gameobject to the weapon info's dictionary.
    // weaponInfo.Add(this, col);
    // AddWeaponInfoToDictionary(this);
    weaponInfo.Add(this, transform);

  }


  protected void OnEnterTransform(Transform t)
  {
    if (ShotDamageableDictionary.ContainsActive(t))
    {
      IShotDamageable ec = ShotDamageableDictionary.GetActive(t);
      EnteredEnemies.Add(ec);
    }
  }

  protected void OnExitTransform(Transform t)
  {
    if (ShotDamageableDictionary.Contains(t))
    {
      // Debug.Log("Remove on exit.");
      IShotDamageable ec = ShotDamageableDictionary.Get(t);
      // DamagedEnemies.Remove(ec);
      ExitedEnemies.Add(ec);
    }
  }

  void OnEnemyReleased(IShotDamageable ec)
  {
    ec.OnEnemyReleased -= OnEnemyReleased;
    ReleasedEnemies.Add(ec);
    // Debug.Log("Released.");
  }

  public Transform GetTarget()
  {
    if (weaponInfo is TargetedWeaponInfo)
    {
      return ((TargetedWeaponInfo)weaponInfo).target;
    }
    return null;
  }

  public float GetShotDamage()
  {
    return weaponInfo.GetShotDamage();
  }

  public Transform GetTransform()
  {
    return this.transform;
  }

  public float GetShotKnockback()
  {
    return weaponInfo.GetShotKnockback();
  }
}
