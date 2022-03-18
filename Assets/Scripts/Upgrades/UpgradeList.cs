using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class UpgradeList : MonoBehaviour
{
  [SerializeField] List<Upgrade> PlayerUpgrades = new List<Upgrade>();

  [SerializeField] List<Upgrade> WeaponUpgrades = new List<Upgrade>();

  [SerializeField] float OldUpgradeChance = 0.5f;
  [SerializeField] float WeaponUpgradeChance = 0.5f;

  [Header("Debug")]
  [SerializeField] List<Upgrade> UsedPlayerUpgrades = new List<Upgrade>();
  [SerializeField] List<Upgrade> UsedWeaponUpgrades = new List<Upgrade>();


  [SerializeField] int upgradesPerLevel = 2;


  private void Awake()
  {
    LevelUpUI.OnUpgradeChosenAction += OnUpgradeChosenActionHandler;
  }

  void OnUpgradeChosenActionHandler(Upgrade upgrade)
  {
    if (upgrade is WeaponUpgrade)
    {
      if (!UsedWeaponUpgrades.Contains(upgrade))
      {
        UsedWeaponUpgrades.Add(upgrade);
      }
    }
    else
    {
      if (!UsedPlayerUpgrades.Contains(upgrade))
      {
        UsedPlayerUpgrades.Add(upgrade);
      }
    }
  }

  Upgrade GetRandom(List<Upgrade> list)
  {
    return list[UnityEngine.Random.Range(0, list.Count)];
  }

  /// <summary>
  /// Somehow getting duplicate entries
  /// </summary>
  /// <returns></returns>
  public List<Upgrade> GetUpgradesToDisplay()
  {
    List<Upgrade> ups = new List<Upgrade>();


    List<Upgrade> alreadyUpgradedPlayer = new List<Upgrade>(UsedPlayerUpgrades);
    List<Upgrade> alreadyUpgradedWeapons = new List<Upgrade>(UsedWeaponUpgrades);


    List<Upgrade> newPlayerUpgrades = PlayerUpgrades.Except(UsedPlayerUpgrades).ToList();
    List<Upgrade> newWeaponUpgrades = WeaponUpgrades.Except(UsedWeaponUpgrades).ToList();

    int i = alreadyUpgradedPlayer.RemoveAll(item => !item.CanBeUpgraded());
    int b = alreadyUpgradedWeapons.RemoveAll(item => !item.CanBeUpgraded());


    Debug.Log("Removed already upgraded p:" + i + " w: " + b);
    while (ups.Count < upgradesPerLevel)
    {
      bool useNewUpgrade = UnityEngine.Random.Range(0f, 1f) > OldUpgradeChance ? true : false;
      bool useWeaponUpgrade = UnityEngine.Random.Range(0f, 1f) > WeaponUpgradeChance ? false : true;
      if (alreadyUpgradedPlayer.Count == 0)
      {
        useNewUpgrade = true;
      }
      Upgrade u = null;
      if (useNewUpgrade)
      {
        if ((useWeaponUpgrade || newPlayerUpgrades.Count == 0) && newWeaponUpgrades.Count > 0)
        {
          u = GetRandom(newWeaponUpgrades);
          if (!newWeaponUpgrades.Remove(u))
          {
            LogFailedRemove(u, newWeaponUpgrades);
          }
        }
        else if (newPlayerUpgrades.Count > 0)
        {
          u = GetRandom(newPlayerUpgrades);
          if (!newPlayerUpgrades.Remove(u))
          {
            LogFailedRemove(u, newPlayerUpgrades);
          }
        }
      }
      if (!useNewUpgrade || u == null)
      {
        if ((useWeaponUpgrade || alreadyUpgradedPlayer.Count == 0) && alreadyUpgradedWeapons.Count > 0)
        {
          u = GetRandom(alreadyUpgradedWeapons);
          if (!alreadyUpgradedWeapons.Remove(u))
          {
            LogFailedRemove(u, alreadyUpgradedWeapons);
          }
        }
        else if (alreadyUpgradedPlayer.Count > 0)
        {
          u = GetRandom(alreadyUpgradedPlayer);
          if (!alreadyUpgradedPlayer.Remove(u))
          {
            LogFailedRemove(u, alreadyUpgradedPlayer);
          }
        }
      }
      if (u != null)
      {
        ups.Add(u);
      }
      else
      {
        break;
      }
    }
    return ups;
  }

  void LogFailedRemove(Upgrade u, List<Upgrade> list)
  {
    Debug.Log("Failed to remove " + u.name + " from " + list);
  }
}
