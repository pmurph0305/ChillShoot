using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class HomingShot : PrefabShot
{
  public override void OnGetFromPool()
  {
    base.OnGetFromPool();
    Transform trackedObject = ((TargetedWeaponInfo)weaponInfo).target;
    if (trackedObject != null)
    {
      travelDirection = (trackedObject.position - transform.position).normalized;
    }
    else
    {
      travelDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    }
  }
}
