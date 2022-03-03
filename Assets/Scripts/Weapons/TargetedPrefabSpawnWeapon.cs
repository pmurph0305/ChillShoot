using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedPrefabSpawnWeapon : PrefabSpawnWeapon
{
  [SerializeField] float targetUpdateTimer = 0.2f;
  Timer targetUpdater;

  TargetedWeaponInfo targetedWeaponInfo;

  protected override void OnStart()
  {
    base.OnStart();
    targetUpdater = new Timer(targetUpdateTimer);
    targetedWeaponInfo = (TargetedWeaponInfo)weaponInfo;
    targetedWeaponInfo.weaponTransform = this.transform;
  }

  protected override void OnUpdate()
  {
    if (targetUpdater.Update() || targetedWeaponInfo.NeedsUpdate())
    {
      targetedWeaponInfo.UpdateTarget();
      targetUpdater.Reset(false);
    }
  }
}
