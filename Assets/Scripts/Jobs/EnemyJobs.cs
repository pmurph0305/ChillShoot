using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using System;
public class EnemyJobs : MonoBehaviour
{
  // barely any difference using jobs vs updating every frame in on each enemy controller, the expensive part is the physics.

  public Transform player;
  TransformAccessArray EnemyTransforms;
  public int currentIndex;
  NativeList<EnemyStruct> EnemyList;

  [System.Serializable]
  struct EnemyStruct
  {
    public float3 direction;
    public float3 position;
  }


  JobHandle jobHandle;

  public static EnemyJobs instance { get; private set; }
  private void Awake()
  {
    if (instance != null && instance != this)
    {
      Destroy(this);
    }
    else
    {
      instance = this;
    }
  }

  void Start()
  {
    EnemyList = new NativeList<EnemyStruct>(Allocator.Persistent);
    EnemyTransforms = new TransformAccessArray(1000);
  }

  private void OnDestroy()
  {
    if (!jobHandle.IsCompleted) jobHandle.Complete();
    if (EnemyTransforms.isCreated) EnemyTransforms.Dispose();
    if (EnemyList.IsCreated) EnemyList.Dispose();
  }


  Queue<EnemyController> QueuedEnemys = new Queue<EnemyController>();

  public void RegisterEnemy(EnemyController enemy)
  {
    // Debug.Log("Register enemy");
    QueuedEnemys.Enqueue(enemy);
  }


  public Vector3 GetDirection(int JobIndex)
  {
    if (JobIndex < 0 || JobIndex >= EnemyList.Length) return Vector3.zero;
    return EnemyList[JobIndex].direction;
  }

  void AddEnemy(EnemyController enemy)
  {
    int lLength = EnemyList.Length;
    int tLength = EnemyTransforms.length;
    // if (lLength >= tLength) return;
    Debug.Assert(tLength == lLength);
    EnemyTransforms.Add(enemy.transform);
    EnemyList.Add(new EnemyStruct());
    enemy.JobIndex = lLength;
    currentIndex = lLength;
    // Debug.Log("add enemy.");
  }


  void Update()
  {
    if (!jobHandle.IsCompleted)
    {
      jobHandle.Complete();
    }

    while (QueuedEnemys.Count > 0)
    {
      AddEnemy(QueuedEnemys.Dequeue());
    }



    DirectionUpdateJob du = new DirectionUpdateJob()
    {
      EnemyList = EnemyList,
      playerPosition = new float3(player.position)
    };
    jobHandle = du.Schedule(EnemyList.Length, 64, jobHandle);

    WritePositionsJob wpj = new WritePositionsJob()
    {
      EnemyList = EnemyList
    };

    jobHandle = wpj.Schedule(EnemyTransforms, jobHandle);


    JobHandle.ScheduleBatchedJobs();
  }

  void LateUpdate()
  {
    jobHandle.Complete();
  }

  struct WritePositionsJob : IJobParallelForTransform
  {
    public NativeArray<EnemyStruct> EnemyList;

    public void Execute(int index, TransformAccess transform)
    {
      if (!transform.isValid) return;
      EnemyStruct s = EnemyList[index];
      s.position = transform.position;
      EnemyList[index] = s;
    }
  }

  // [BurstCompile]
  struct DirectionUpdateJob : IJobParallelFor
  {
    public NativeArray<EnemyStruct> EnemyList;
    [ReadOnly]
    public float3 playerPosition;

    public void Execute(int index)
    {
      EnemyStruct s = EnemyList[index];
      float3 dir = playerPosition - s.position;
      // float3 normalized = math.sqrt(dirToPlayer);
      float len = math.sqrt((dir.x * dir.x) + (dir.y * dir.y) + (dir.z * dir.z));
      dir /= len;
      s.direction = dir;
      EnemyList[index] = s;
    }
  }

}
