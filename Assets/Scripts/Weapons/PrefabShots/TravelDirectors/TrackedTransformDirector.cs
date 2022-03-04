using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedTransformDirector : TravelDirector
{
  protected override Vector3 GetNewTravelDirection(WeaponInfo weaponInfo)
  {
    Transform trackedObject = ((TargetedWeaponInfo)weaponInfo).target;
    if (trackedObject != null)
    {
      Vector3 v = (trackedObject.position - transform.position).normalized;
      Debug.DrawLine(transform.position, transform.position + travelDirection, Color.blue, 1f);
      return v;
    }
    else
    {
      // Debug.Log("not tracking.");
      return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }
  }
}
