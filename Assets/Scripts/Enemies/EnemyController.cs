using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using System;
public class EnemyController : MonoBehaviour, IPoolable<EnemyController>
{
  [SerializeField] public float speed = 1;

  [SerializeField] float damage = 10;

  [SerializeField] float damageTimerLength = 0.2f;
  [SerializeField] float MaxHealth = 100;
  [SerializeField] float health = 100;

  [SerializeField] float expValue = 10;
  Timer damageTimer;

  [Tooltip("Time between player direction updates"), SerializeField]
  float UpdateDirToPlayerTime = 0.0133f;

  Timer updateDirectionTimer;
  Transform t;
  [SerializeField] Rigidbody2D rb2d;
  [SerializeField] Collider2D col;

  [SerializeField] NavMeshAgent agent;
  [SerializeField] Vector3 dir;

  public int JobIndex;

  public bool useAgent;


  [SerializeField] float updatePathTime = 1f;
  Timer agentPathTimer;

  public event Action OnEnemyReleased;
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
      Vector3 toPlayer = (PlayerController.PlayerPosition - t.position);
      dir = (toPlayer).normalized;
      // dir = toPlayer.FastNormalized();
      // why fixed delta time?
      Vector3 position = t.position + speed * Time.fixedDeltaTime * dir;
      rb2d.MovePosition(position);
      damageTimer.FixedUpdate();
    }
  }


  void TakeDamage(float damage)
  {
    // Debug.Log("Take damage:" + damage);
    health -= damage;
    if (health < 0)
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
    // Debug.Log("Release");
    pool.Release(this);
    EnemyDictionary.Remove(col);
    OnEnemyReleased?.Invoke();
  }

  private void OnTriggerStay2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      // somehow can be null?
      if (damageTimer == null || !damageTimer.IsFinished) return;
      damageTimer.Reset(true);
      PlayerController pc = other.gameObject.GetComponent<PlayerController>();
      pc.TakeDamage(damage);
    }
    else if (pool != null)
    {
      WeaponInfo info = WeaponDictionary.Get(other.gameObject.tag);
      TakeDamage(info.ShotDamage);
      // Destroy(other.gameObject);
      info.OnHitEnemy(other);
    }
  }

  // todo: if on trigger stay is used damage is taken every frame, need to be able to only take damage occasionally depending on weapon.
  private void OnTriggerEnter2D(Collider2D other)
  {

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
    EnemyDictionary.Add(col, this);
  }

  IObjectPool<EnemyController> pool;
  public void SetPool(IObjectPool<EnemyController> pool)
  {
    this.pool = pool;
  }

  public void OnCreate()
  {
    t = this.transform;
    updateDirectionTimer = new Timer(UpdateDirToPlayerTime);
    damageTimer = new Timer(damageTimerLength);
    if (useAgent)
    {
      Destroy(GetComponent<Rigidbody2D>());
    }
    else
    {
      Destroy(GetComponent<NavMeshAgent>());
    }

    if (agent != null)
    {
      agentPathTimer = new Timer(updatePathTime);
      agent.updateRotation = false;
      agent.updateUpAxis = false;
    }
  }
}
