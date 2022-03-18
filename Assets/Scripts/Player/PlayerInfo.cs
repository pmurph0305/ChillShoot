using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerInfo : MonoBehaviour
{
  public static PlayerInfo instance;

  public static PlayerData Data => instance.pData;

  public PlayerData pData;

  public static float GetCooldownMultiplier()
  {
    return instance.pData.CooldownMultiplier;
  }

  public static float GetAreaMultiplier()
  {
    return instance.pData.AreaMultiplier;
  }

  public static float GetDurationMultiplier()
  {
    return instance.pData.DurationMultiplier;
  }

  public float GetPlayerSpeed()
  {
    return pData.Speed;
  }



  public void OnPlayerGainExpActionHandler(float value)
  {
    pData.GainExp(value);
  }



  public static Action<PlayerUpgradeType, float> OnPlayerUpgradeAction;



  private void Awake()
  {
    if (instance != null && instance != this)
    {
      Debug.LogError("Duplicate player data", this.gameObject);
    }
    else
    {
      instance = this;
    }
    PlayerUpgrade.OnPlayerUpgradeEvent += OnPlayerUpgradeActionHandler;
    ExperiencePickerUpper.OnPlayerGainExpAction += OnPlayerGainExpActionHandler;
  }



  void OnPlayerUpgradeActionHandler(PlayerUpgradeType type, float amount)
  {
    switch (type)
    {
      case PlayerUpgradeType.Health:
        pData.IncreaseHealth(amount);
        break;
      case PlayerUpgradeType.Cooldown:
        pData.ApplyCooldown(amount);
        break;
      case PlayerUpgradeType.Area:
        pData.ApplyArea(amount);
        break;
      case PlayerUpgradeType.Duration:
        pData.ApplyDuration(amount);
        break;
      case PlayerUpgradeType.Speed:
        pData.ApplySpeed(amount);
        break;
      case PlayerUpgradeType.ExpGain:
        pData.ApplyExperienceUpgrade(amount);
        break;
    }
  }
}

