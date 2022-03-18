using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(menuName = "Upgrades/Player")]
public class PlayerUpgrade : Upgrade
{
  [Header("Upgrade values")]
  [SerializeField] PlayerUpgradeType upgradeType;
  [SerializeField] List<float> Values = new List<float>(1) { 0.1f };

  [SerializeField] string DisplayString;
  [SerializeField] string DisplaySuffix = "%";



  public static event Action<PlayerUpgradeType, float> OnPlayerUpgradeEvent;
  public override void ApplyUpgrade()
  {
    if (currentUpgrade < Values.Count)
    {
      // PlayerInfo.OnPlayerUpgradeAction(upgradeType, Values[currentUpgrade]);
      OnPlayerUpgradeEvent?.Invoke(upgradeType, Values[currentUpgrade]);
    }
    currentUpgrade++;
  }

  public override bool CanBeUpgraded()
  {
    return currentUpgrade < Values.Count;
  }

  public float GetPercentUpgrade()
  {
    return (Values[currentUpgrade] * 100);
  }

  public override string GetDisplayString()
  {
    // Debug.Log(currentUpgrade.ToString() + ":" + DisplayString);
    return DisplayString + " +" + GetPercentUpgrade().ToString() + DisplaySuffix;
  }
}