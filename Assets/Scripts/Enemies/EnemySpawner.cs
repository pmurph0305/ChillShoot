using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  [SerializeField] EnemyPool enemyPool;
  Timer spawnTimer;
  [SerializeField] float spawnTime = 0.5f;
  [SerializeField] protected int _EnemyCount;
  public int EnemyCount { get { return _EnemyCount; } protected set { _EnemyCount = value; } }
  [SerializeField] int maxEnemyCount = 1000;

  protected virtual Vector3 GetSpawnPosition()
  {
    return transform.position;
  }

  private void Start()
  {
    OnStart();
  }

  protected virtual void OnStart()
  {
    spawnTimer = new Timer(spawnTime);
    enemyPool.OnGetFromPoolAction += OnGetFromPool;
    enemyPool.OnReturnedToPoolAction += OnReturnedToPool;
  }

  private void OnDestroy()
  {
    OnDestroyed();
  }

  protected virtual void OnDestroyed()
  {
    enemyPool.OnGetFromPoolAction -= OnGetFromPool;
    enemyPool.OnReturnedToPoolAction -= OnReturnedToPool;
  }

  protected virtual void OnGetFromPool(EnemyController obj)
  {
    EnemyCount++;
  }

  protected virtual void OnReturnedToPool(EnemyController obj)
  {
    EnemyCount--;
  }

  protected virtual void OnSpawnTimerFinished()
  {
    Spawn();
  }

  protected virtual void Spawn()
  {
    enemyPool.Get(GetSpawnPosition());
  }

  private void Update()
  {
    OnUpdate();
  }

  protected virtual void OnUpdate()
  {
    if (spawnTimer.EndTime != spawnTime)
    {
      spawnTimer.SetEndTime(spawnTime);
    }
    if (EnemyCount < maxEnemyCount && spawnTimer.Update())
    {
      spawnTimer.Reset(false);
      OnSpawnTimerFinished();
    }
  }

}
