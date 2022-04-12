using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavMeshDirector : TrackedTransformDirector
{
  NavMeshPath path;
  [Header("Nav Mesh Director")]
  [SerializeField] float Radius = 1.0f;
  float SqrRadius = 0.0f;
  int currentIndex;
  protected override Vector3 GetNewTravelDirection()
  {
    previousLocation = transform.position;
    SqrRadius = Radius * Radius;
    NavMeshHit navHit;
    bool sample = NavMesh.SamplePosition(transform.position, out navHit, 1f, NavMesh.AllAreas);
    path = new NavMeshPath();
    bool pathFound = NavMesh.CalculatePath(transform.position, targetProvider.GetTarget().position, NavMesh.AllAreas, path);
    if (pathFound)
    {
      currentIndex = 0;
      return (GetCorner(currentIndex) - transform.position).normalized;
    }
    else
    {
      Debug.Log("no path", this.gameObject);
      currentIndex = -1;
      return Vector3.zero;
    }
  }

  protected Vector3 GetCorner(int index)
  {
    if (index < 0) return transform.position;
    return path.corners[index];
  }

  protected int GetNextCorner(int currentIndex)
  {
    if (currentIndex + 1 >= path.corners.Length)
    {
      return -1;
    }
    return currentIndex + 1;
  }

  [SerializeField] float minMoveDist = 0.01f;
  [SerializeField] int sequentialMinimum;
  [SerializeField] int MaxSequentialMinsBeforeRepath = 10;
  Vector3 previousLocation;
  public override Vector3 GetTravelDirection()
  {
    // return base.GetTravelDirection();
    float d = Vector3.SqrMagnitude(transform.position - GetCorner(currentIndex));
    float distMoved = Vector3.SqrMagnitude(transform.position - previousLocation);
    previousLocation = transform.position;
    if (distMoved < minMoveDist)
    {
      sequentialMinimum++;
    }
    else
    {
      sequentialMinimum = 0;
    }
    if (sequentialMinimum > MaxSequentialMinsBeforeRepath)
    {
      Debug.Log("Repath.");
      sequentialMinimum = 0;
      travelDirection = GetNewTravelDirection();
      return travelDirection;
    }


    if (d < SqrRadius)
    {
      currentIndex = GetNextCorner(currentIndex);
      if (currentIndex < 0)
      {
        travelDirection = GetNewTravelDirection();
      }
      else
      {
        travelDirection = (GetCorner(currentIndex) - transform.position).normalized;
      }
      return travelDirection;
    }
    return (GetCorner(currentIndex) - transform.position).normalized;
  }
}
