using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedWeaponInfoComponent : BaseWeaponInfoContainer
{
  [SerializeField] TargetedWeaponInfo weaponInfo;

  public override WeaponInfo GetWeaponInfo()
  {
    return weaponInfo;
  }
}
