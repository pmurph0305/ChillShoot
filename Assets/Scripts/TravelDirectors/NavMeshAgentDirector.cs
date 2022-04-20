using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshAgentDirector : TrackedTransformDirector
{
  NavMeshPath path;
  [Header("Nav Mesh Director")]
  [SerializeField] NavMeshAgent agent;
  [SerializeField] bool UpdatePosition;
  float SqrStoppingDistance;
  [SerializeField] float minWarpDistanceSqr = 0.1f;
  public override void ResetTravelDirector()
  {
    base.ResetTravelDirector();
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
        Debug.Log("Agent warped to position.");
      }
    }
    if (agent.isOnNavMesh)
    {
      agent.SetDestination(targetProvider.GetTarget().position);
    }
    return Vector3.zero;
  }


  [SerializeField] NavMeshDebug debug;

  [SerializeField] float RecalcTime = 1.0f;

  float t = 0;
  public override Vector3 GetTravelDirection()
  {
    // travelDirection = Vector3.MoveTowards(travelDirection, (agent.nextPosition - transform.position).normalized, 1f * Time.deltaTime);
    travelDirection = agent.desiredVelocity.normalized;
    return travelDirection;
  }

  protected void RecalculatePath()
  {
    if (!agent.isOnNavMesh)
    {
      Warp();
    }
    if (agent.isOnNavMesh)
    {
      agent.SetDestination(targetProvider.GetTarget().position);
    }
  }

  protected void Warp()
  {
    NavMeshHit hit;
    if (NavMesh.SamplePosition(transform.position, out hit, 1f, NavMesh.AllAreas))
    {
      // Debug.Log("Sampled.");
      // transform.position = hit.position;
      if (agent.Warp(hit.position))
      {
        Debug.Log("Agent warped to position.");
      }
      RecalculatePath();
    }
  }

  private void PreMoveChecks(float deltaTime)
  {
    t += deltaTime;
    if (t >= RecalcTime || agent.isStopped || agent.isPathStale || !agent.isOnNavMesh)
    {
      RecalculatePath();
      t = 0;
    }
    float distance = Vector3.SqrMagnitude(agent.nextPosition - transform.position);
    if (distance > minWarpDistanceSqr)
    {
      Warp();
    }
  }

  protected override void UpdateMovement(Rigidbody2D rb2d, float movementSpeed, float deltaTime)
  {
    UpdateMovement(movementSpeed, deltaTime, true);
  }

  protected override void UpdateMovement(Rigidbody rb, float movementSpeed, float deltaTime)
  {
    UpdateMovement(movementSpeed, deltaTime, false);
  }


  public override void UpdateMovement(float movementSpeed, float deltaTime, bool is2d)
  {
    PreMoveChecks(deltaTime);
    agent.Move(instantVelocity + GetOffset(deltaTime) + additionalVelocity * deltaTime);
    Vector3 val = agent.desiredVelocity;
    if (FaceTravelDirection && visual != null && deltaTime > 0)
    {
      visual.rotation = Quaternion.LookRotation(Vector3.forward, val);
    }
    UpdateRotation(deltaTime, is2d);
  }

  void SetDebugValues()
  {
    debug.isPathStale = agent.isPathStale;
    debug.isStopped = agent.isStopped;
    debug.remainingDistance = agent.remainingDistance;
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + GetTravelDirection());
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