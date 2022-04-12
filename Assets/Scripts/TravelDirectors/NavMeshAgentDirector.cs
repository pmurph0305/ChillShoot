using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentDirector : TrackedTransformDirector
{
  NavMeshPath path;
  [Header("Nav Mesh Director")]
  [SerializeField] float Radius = 1.0f;
  float SqrStoppingDistance = 0.0f;
  int currentIndex;
  [SerializeField] NavMeshAgent agent;
  [SerializeField] bool UpdatePosition;
  public override void OnGetFromPool()
  {
    recalcTimer = new Timer(RecalcTime);
    base.OnGetFromPool();
    SqrStoppingDistance = agent.stoppingDistance * agent.stoppingDistance;
    agent.updatePosition = UpdatePosition;
    agent.updateRotation = false;
    agent.updateUpAxis = false;
    if (GetComponent<NavMeshAgent>() == null)
    {
      Debug.LogError("No nav mesh agent found.", this.gameObject);
    }
  }

  protected override Vector3 GetNewTravelDirection()
  {
    NavMeshHit hit;
    if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
    {
      // Debug.Log("Sampled.");
      transform.position = hit.position;
      if (agent.Warp(hit.position))
      {
        // Debug.Log("Agent warped to position.");
      }
    }
    if (agent.isOnNavMesh)
    {
      agent.SetDestination(targetProvider.GetTarget().position);
    }
    SqrStoppingDistance = Radius * Radius;
    return Vector3.zero;
  }


  [SerializeField] NavMeshDebug debug;

  [SerializeField] float RecalcTime = 1.0f;
  Timer recalcTimer;
  public override Vector3 GetTravelDirection()
  {
    return (agent.nextPosition - transform.position).normalized;
  }

  protected void RecalculatePath()
  {
    if (!agent.isOnNavMesh)
    {
      NavMeshHit hit;
      if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
      {
        // Debug.Log("Sampled.");
        transform.position = hit.position;
        if (agent.Warp(hit.position))
        {
          // Debug.Log("Agent warped to position.");
        }
      }
    }
    if (agent.isOnNavMesh)
    {
      agent.SetDestination(targetProvider.GetTarget().position);
    }
  }

  public override void UpdateMovement(Rigidbody2D rb2d, float movementSpeed, float deltaTime)
  {
    recalcTimer.Update(deltaTime);
    if (recalcTimer.IsFinished || agent.isStopped || agent.isPathStale || !agent.isOnNavMesh)
    {
      RecalculatePath();
      recalcTimer.Reset();
    }
    base.UpdateMovement(rb2d, movementSpeed, deltaTime);
    // if (agent.isStopped || agent.isPathStale || !agent.hasPath)
    // {
    //   Debug.Log("update movement." + travelDirection);
    //   // base.UpdateMovement(rb2d, movementSpeed, deltaTime);
    // }
    // else
    // {
    //   travelDirection = (agent.nextPosition - transform.position).normalized;
    //   // rb2d.MovePosition(agent.nextPosition);
    // }
    // rb2d.MovePosition(agent.nextPosition);
  }

  void SetDebugValues()
  {
    debug.isPathStale = agent.isPathStale;
    debug.isStopped = agent.isStopped;
    debug.remainingDistance = agent.remainingDistance;
  }
}

[System.Serializable]
public class NavMeshDebug
{
  public bool isPathStale;
  public bool isStopped;
  public float remainingDistance;

  public override string ToString()
  {
    return base.ToString() + " RemainingDistance:" + remainingDistance.ToString();
  }
}