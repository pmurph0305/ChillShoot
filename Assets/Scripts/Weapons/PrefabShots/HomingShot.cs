using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class HomingShot : PrefabShot
{
  Transform tracked;
  public override void OnGetFromPool()
  {
    base.OnGetFromPool();
    Transform trackedObject = ((TargetedWeaponInfo)weaponInfo).target;
    tracked = trackedObject;
    if (trackedObject != null)
    {
      travelDirection = (trackedObject.position - transform.position).normalized;
      Debug.DrawLine(transform.position, transform.position + travelDirection, Color.blue, 1f);
    }
    else
    {
      // Debug.Log("not tracking.");
      travelDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
    }
  }

  protected override void OnUpdate()
  {
    base.OnUpdate();
    if (tracked != null)
    {
      Debug.DrawLine(transform.position, tracked.position, Color.red);
    }
  }
}
