using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePrefabSpawnWeapon : PrefabSpawnWeapon
{
  [SerializeField] float distancePerShot;

  float GetRequiredDistance()
  {
    return distancePerShot * PlayerInfo.GetCooldownMultiplier();
  }

  Vector3 previousPosition;
  float distance;
  protected override void OnUpdate()
  {
    base.OnUpdate();
    distance += Vector3.Distance(transform.position, previousPosition);
    previousPosition = transform.position;
    if (distance > GetRequiredDistance())
    {
      Shoot();
      distance -= GetRequiredDistance();
    }
  }

  protected override void UpdateCooldownTimer()
  {
    // distance, so no cooldown timer.
    // base.UpdateCooldownTimer();
  }

  protected override void OnStart()
  {
    base.OnStart();
    previousPosition = transform.position;
  }
}
