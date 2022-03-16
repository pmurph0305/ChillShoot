using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : Upgrade
{
  public WeaponKey weaponKey;

  [SerializeField] protected List<WeaponUpgradeItem> Upgrades = new List<WeaponUpgradeItem>();
  public override void ApplyUpgrade()
  {
    throw new System.NotImplementedException();
  }

  public override bool CanBeUpgraded()
  {
    throw new System.NotImplementedException();
  }

  public override string GetDisplayString()
  {
    throw new System.NotImplementedException();
  }

}

[System.Serializable]
public class WeaponUpgradeItem
{

}
