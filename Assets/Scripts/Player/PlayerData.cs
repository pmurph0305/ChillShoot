using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerData : MonoBehaviour
{
  public static float CooldownMultiplier = 1.0f;
  public static float AreaMultiplier = 1.0f;
  public static float DurationMultiplier = 1.0f;

  public static Action<PlayerUpgradeType, float> OnPlayerUpgradeAction;

  private void Awake()
  {
    OnPlayerUpgradeAction += OnPlayerUpgradeActionHandler;
  }

  void IncreaseHealth(float amount)
  {
    PlayerController.OnHealthUpgrade.Invoke(amount);
  }

  void ApplyCooldown(float amount)
  {
    CooldownMultiplier -= amount;
  }

  void ApplyArea(float amount)
  {
    AreaMultiplier += amount;
  }

  void ApplyDuration(float amount)
  {
    DurationMultiplier += amount;
  }

  void ApplySpeed(float amount)
  {
    PlayerController.OnSpeedUpgrade.Invoke(amount);
  }

  void OnPlayerUpgradeActionHandler(PlayerUpgradeType type, float amount)
  {
    switch (type)
    {
      case PlayerUpgradeType.Health:
        IncreaseHealth(amount);
        break;
      case PlayerUpgradeType.Cooldown:
        ApplyCooldown(amount);
        break;
      case PlayerUpgradeType.Area:
        ApplyArea(amount);
        break;
      case PlayerUpgradeType.Duration:
        ApplyDuration(amount);
        break;
      case PlayerUpgradeType.Speed:
        ApplySpeed(amount);
        break;
    }
  }
}
