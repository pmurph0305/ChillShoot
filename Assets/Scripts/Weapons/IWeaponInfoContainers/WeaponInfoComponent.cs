using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfoComponent : BaseWeaponInfoContainer
{
  public WeaponInfo weaponInfo;
  public override WeaponInfo GetWeaponInfo()
  {
    return weaponInfo;
  }
}
