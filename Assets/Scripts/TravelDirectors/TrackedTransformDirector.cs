using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Updates the direction to travel only when getting a new travel direction (like when OnGetFromPool is called)
/// </summary>
public class TrackedTransformDirector : TravelDirector
{
  private void Awake()
  {
    targetProvider = GetComponent<ITargetProvider>();
  }

  [SerializeField] protected ITargetProvider targetProvider;
  [SerializeField, Tooltip("Just visible for debugging, automatically set by an ITargetProvider attached to this object")] protected Transform target;
  protected override Vector3 GetNewTravelDirection()
  {
    target = targetProvider.GetTarget();
    if (target != null)
    {
      Vector3 v = (target.position - transform.position).normalized;
      // Debug.DrawLine(transform.position, transform.position + travelDirection, Color.blue, 1f);
      return v;
    }
    else
    {
      // Debug.Log("not tracking.");
      return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }
  }
}
