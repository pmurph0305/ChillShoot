using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class UpgradeList : MonoBehaviour
{
  [SerializeField] List<Upgrade> AllUpgrades = new List<Upgrade>();

  [Header("Debug")]
  [SerializeField] List<Upgrade> UsedUpgrades = new List<Upgrade>();


  [SerializeField] int upgradesPerLevel = 2;

  public static Action<Upgrade> OnUpgradeChosenAction;
  private void Awake()
  {
    OnUpgradeChosenAction += OnUpgradeChosenActionHandler;
  }

  void OnUpgradeChosenActionHandler(Upgrade upgrade)
  {
    UsedUpgrades.Add(upgrade);
  }

  Upgrade GetRandom(List<Upgrade> list)
  {
    return list[UnityEngine.Random.Range(0, list.Count)];
  }


  public List<Upgrade> GetUpgradesToDisplay()
  {
    List<Upgrade> ups = new List<Upgrade>();

    List<Upgrade> alreadyUpgraded = new List<Upgrade>(UsedUpgrades);
    List<Upgrade> notYetUpgraded = AllUpgrades.Except(UsedUpgrades).ToList();
    int i = alreadyUpgraded.RemoveAll(item => !item.CanBeUpgraded());
    Debug.Log("Removed already upgraded:" + i);
    while (ups.Count < upgradesPerLevel)
    {
      bool useNewUpgrade = UnityEngine.Random.Range(0f, 1f) > 0.5f ? true : false;
      if (alreadyUpgraded.Count == 0)
      {
        useNewUpgrade = true;
      }
      Upgrade u = null;
      if (useNewUpgrade && notYetUpgraded.Count > 0)
      {
        u = GetRandom(notYetUpgraded);
        notYetUpgraded.Remove(u);
      }
      if (!useNewUpgrade || u == null && alreadyUpgraded.Count > 0)
      {
        u = GetRandom(alreadyUpgraded);
        alreadyUpgraded.Remove(u);
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
