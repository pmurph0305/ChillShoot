using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Upgrades/Weapon")]
public class WeaponUpgrade : Upgrade
{
  public WeaponKey weaponKey;

  [SerializeField] GameObject prefab;
  [SerializeField] string DisplayString = "";
  [SerializeField] protected List<WeaponUpgradeLevel> Values = new List<WeaponUpgradeLevel>();

  public static event Action<WeaponUpgrade> OnPlayerAcquireNewWeaponAction;
  public override void ApplyUpgrade()
  {
    if (WeaponDictionary.Contains(weaponKey))
    {
      WeaponInfo info = WeaponDictionary.Get(weaponKey);
      if (currentUpgrade < Values.Count)
      {
        // PlayerData.OnPlayerUpgradeAction(upgradeType, Values[currentUpgrade]);
        foreach (var item in Values[currentUpgrade].Upgrades)
        {
          item.ApplyUpgrade(info);
        }
      }
      currentUpgrade++;
    }
    else
    {
      OnPlayerAcquireNewWeaponAction?.Invoke(this);
    }

  }

  public override bool CanBeUpgraded()
  {
    return currentUpgrade < Values.Count;
  }

  public override string GetDisplayString()
  {
    string s = "";
    try
    {
      foreach (var item in Values[currentUpgrade].Upgrades)
      {
        s += "- " + item.GetDisplayString() + "\n";
      }
      return currentUpgrade.ToString() + ":" + DisplayString + "\n" + s;
    }
    catch (Exception e)
    {
      throw new Exception(this.weaponKey.ToString() + " : " + currentUpgrade + " : " + Values.Count, e);
    }
  }

}

[System.Serializable]
public class WeaponUpgradeLevel
{
  public List<WeaponUpgradeItem> Upgrades;
}

[System.Serializable]
public class WeaponUpgradeItem
{
  [SerializeField] WeaponUpgradeType upgradeType;
  [SerializeField] float value;

  public string GetDisplayString()
  {
    return upgradeType.ToString() + " " + value;
  }
  public void ApplyUpgrade(WeaponInfo info)
  {
    info.ApplyUpgrade(upgradeType, value);
  }
}

public enum WeaponUpgradeType
{
  None,
  Damage,
  Speed,
  Area,
  Cooldown,
  AdditionalProjectiles,
  AdditionalHitsBeforeDestroy
}