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
      UsedWeaponUpgrades.Add(upgrade);
    }
    else
    {
      UsedPlayerUpgrades.Add(upgrade);
    }
  }

  Upgrade GetRandom(List<Upgrade> list)
  {
    return list[UnityEngine.Random.Range(0, list.Count)];
  }


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
          newWeaponUpgrades.Remove(u);
        }
        else if (newPlayerUpgrades.Count > 0)
        {
          u = GetRandom(newPlayerUpgrades);
          newPlayerUpgrades.Remove(u);
        }
      }
      if (!useNewUpgrade || u == null)
      {
        if ((useWeaponUpgrade || alreadyUpgradedPlayer.Count == 0) && alreadyUpgradedWeapons.Count > 0)
        {
          u = GetRandom(alreadyUpgradedWeapons);
          alreadyUpgradedWeapons.Remove(u);
        }
        else if (alreadyUpgradedPlayer.Count > 0)
        {
          u = GetRandom(alreadyUpgradedPlayer);
          alreadyUpgradedPlayer.Remove(u);
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
}
