using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/Player")]
public class PlayerUpgrade : Upgrade
{
  [Header("Upgrade values")]
  [SerializeField] PlayerUpgradeType upgradeType;
  [SerializeField] List<float> Values = new List<float>(1) { 0.1f };

  [SerializeField] string DisplayString;
  public override void ApplyUpgrade()
  {
    currentUpgrade++;
    if (currentUpgrade < Values.Count)
    {
      PlayerData.OnPlayerUpgradeAction(upgradeType, Values[currentUpgrade]);
    }
  }

  public override bool CanBeUpgraded()
  {
    return currentUpgrade + 1 < Values.Count;
  }

  public float GetPercentUpgrade()
  {
    return (Values[currentUpgrade + 1] * 100);
  }

  public override string GetDisplayString()
  {
    Debug.Log(currentUpgrade.ToString() + ":" + DisplayString);
    return currentUpgrade.ToString() + ":" + DisplayString + " " + GetPercentUpgrade().ToString();
  }
}