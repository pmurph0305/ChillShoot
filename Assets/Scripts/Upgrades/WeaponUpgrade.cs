using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/Weapon")]
public class WeaponUpgrade : Upgrade
{
  public WeaponKey weaponKey;
  [SerializeField] string DisplayString = "";
  [SerializeField] protected List<WeaponUpgradeLevel> Values = new List<WeaponUpgradeLevel>();
  public override void ApplyUpgrade()
  {
    WeaponInfo info = WeaponDictionary.Get(weaponKey);
    currentUpgrade++;
    if (currentUpgrade < Values.Count)
    {
      // PlayerData.OnPlayerUpgradeAction(upgradeType, Values[currentUpgrade]);
      foreach (var item in Values[currentUpgrade].Upgrades)
      {
        item.ApplyUpgrade(info);
      }
    }
  }

  public override bool CanBeUpgraded()
  {
    return currentUpgrade + 1 < Values.Count;
  }

  public override string GetDisplayString()
  {
    string s = "";
    foreach (var item in Values[currentUpgrade + 1].Upgrades)
    {
      s += "- " + item.GetDisplayString() + "\n";
    }
    return currentUpgrade.ToString() + ":" + DisplayString + "\n" + s;
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
}