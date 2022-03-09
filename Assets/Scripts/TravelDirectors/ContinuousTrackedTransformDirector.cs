using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Continually updates the direction to the transform.
/// </summary>
public class ContinuousTrackedTransformDirector : TrackedTransformDirector
{
  public override Vector3 GetTravelDirection()
  {
    if (target == null) return travelDirection;
    return (target.position - transform.position).normalized;
  }
}
