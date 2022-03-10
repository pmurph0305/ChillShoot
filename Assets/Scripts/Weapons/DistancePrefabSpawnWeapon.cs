using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePrefabSpawnWeapon : PrefabSpawnWeapon
{
  [SerializeField] float distancePerShot;
  Vector3 previousPosition;
  float distance;
  protected override void OnUpdate()
  {
    base.OnUpdate();
    distance += Vector3.Distance(transform.position, previousPosition);
    previousPosition = transform.position;
    if (distance > distancePerShot)
    {
      Shoot();
      distance -= distancePerShot;
    }
  }

  protected override void OnStart()
  {
    base.OnStart();
    previousPosition = transform.position;
  }
}
