using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using System;
public abstract class EnemyControllerBase : MonoBehaviour, IPoolable<EnemyControllerBase>, ITargetProvider
{
  [SerializeField] protected float speed = 1;

  [SerializeField] float enemyDamage = 10;

  [SerializeField] float damageTimerLength = 0.2f;
  [SerializeField] float MaxHealth = 100;
  [SerializeField] float health = 100;

  [SerializeField] float expValue = 10;
  Timer damageTimer;

  [Tooltip("Time between player direction updates"), SerializeField]
  float UpdateDirToPlayerTime = 0.0133f;

  Timer updateDirectionTimer;
  Transform t;

  IObjectPool<EnemyControllerBase> pool;
  [SerializeField] NavMeshAgent agent;
  [SerializeField] Vector3 dir;

  public bool useAgent;

  [SerializeField] protected Vector3 HitKnockback;
  [SerializeField] float updatePathTime = 1f;
  Timer agentPathTimer;


  [SerializeField] protected TravelDirector travelDirector;
  public event Action<EnemyControllerBase> OnEnemyReleased;

  public static event Action<Vector3, float> OnEnemyDamagedAction;

  // private void Update()
  // {
  //   if (useAgent)
  //   {
  //     if (agentPathTimer.Update())
  //     {
  //       agentPathTimer.Reset(false);
  //       agent.SetDestination(PlayerController.PlayerPosition);
  //     }
  //   }
  //   else
  //   {
  //     Vector3 toPlayer = (PlayerController.PlayerPosition - t.position);
  //     dir = (toPlayer).normalized;
  //     // dir = toPlayer.FastNormalized();
  //     // why fixed delta time?
  //     Vector3 position = t.position + speed * Time.fixedDeltaTime * dir;
  //     rb2d.MovePosition(position);
  //     damageTimer.Update();
  //   }
  // }

  private void FixedUpdate()
  {
    OnFixedUpdate();
  }

  public virtual void OnFixedUpdate()
  {
    if (useAgent)
    {
      if (agentPathTimer.FixedUpdate())
      {
        agentPathTimer.Reset(false);
        agent.SetDestination(PlayerController.PlayerPosition);
      }
    }
    else
    {
      travelDirector.SetInstantVelocity(HitKnockback);
      UpdateMovement(speed, true);
      HitKnockback = Vector3.zero;
      damageTimer.FixedUpdate();
    }
  }


  protected abstract void UpdateMovement(float speed, bool useFixedUpdate);



  public void OnHitFromShot(PrefabShotBase shot)
  {
    TakeDamage(shot.weaponInfo.GetShotDamage());
    HitKnockback += (transform.position - shot.transform.position).normalized * shot.weaponInfo.GetShotKnockback();
  }

  public void OnHitFromAbility(float damage)
  {
    TakeDamage(damage);
  }

  void TakeDamage(float damage)
  {
    // Debug.Log("Take damage:" + damage);
    health -= damage;
    OnEnemyDamagedAction?.Invoke(transform.position, damage);
    if (health <= 0)
    {
      // Destroy(this.gameObject);
      Release();
      // pool = null;
    }
  }

  public void Release()
  {
    ExperiencePickupPool.GetExpItem(transform.position, expValue);
    // use set ondisable to release!
    this.gameObject.SetActive(false);
  }

  private void OnDisable()
  {
    OnDisabled();
  }

  protected virtual void OnDisabled()
  {
    EnemyDictionary.RemoveActive(transform);
    // Debug.Log("Release");
    pool.Release(this);
    OnEnemyReleased?.Invoke(this);
  }

  public void OnGetFromPool()
  {
    updateDirectionTimer.Reset();
    damageTimer.Reset();
    this.health = MaxHealth;
    if (useAgent)
    {
      agent.SetDestination(PlayerController.PlayerPosition);
    }
    EnemyDictionary.AddActive(transform, this);
    travelDirector.OnGetFromPool();
  }


  public void SetPool(IObjectPool<EnemyControllerBase> pool)
  {
    this.pool = pool;
  }

  public void OnCreate()
  {
    EnemyDictionary.Add(transform, this);
    t = this.transform;
    updateDirectionTimer = new Timer(UpdateDirToPlayerTime);
    damageTimer = new Timer(damageTimerLength);
    if (useAgent)
    {
      // Destroy(GetComponent<Rigidbody2D>());
    }
    else
    {
      // Destroy(GetComponent<NavMeshAgent>());
    }
    if (agent != null)
    {
      agentPathTimer = new Timer(updatePathTime);
      agent.updateRotation = false;
      agent.updateUpAxis = false;
    }
  }

  public Transform GetTarget()
  {
    return PlayerController.PlayerTransform;
  }
}
