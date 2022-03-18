using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerInfo : MonoBehaviour
{
  public static PlayerInfo instance;

  public static PlayerData Data => instance.pData;

  public PlayerData pData;

  #region weapons
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

  internal static float GetWeaponSpeedMultiplier()
  {
    return instance.pData.WeaponSpeedMultiplier;
  }

  public static float GetKnockBackMultiplier()
  {
    return instance.pData.KnockbackMultiplier;
  }

  public static float GetDamageMultiplier()
  {
    return instance.pData.DamageMultiplier;
  }

  public static float GetCriticalHitChanceMultiplier()
  {
    return instance.pData.CriticalHitChanceMultiplier;
  }

  public static float GetCritcalDamageMultiplier()
  {
    return instance.pData.CriticalHitDamageMultiplier;
  }
  #endregion


  public float GetPlayerSpeed()
  {
    return pData.Speed;
  }

  public float GetPlayerHealthRegen()
  {
    return pData.HealthRegen;
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
        pData.UpgradeMaxHealth(amount);
        break;
      case PlayerUpgradeType.Cooldown:
        pData.UpgradeCooldownMultiplier(amount);
        break;
      case PlayerUpgradeType.Area:
        pData.UpgradeAreaMultiplier(amount);
        break;
      case PlayerUpgradeType.Duration:
        pData.UpgradeDurationMultiplier(amount);
        break;
      case PlayerUpgradeType.Speed:
        pData.UpgradeMovementSpeed(amount);
        break;
      case PlayerUpgradeType.ExpGain:
        pData.UpgradeExperienceGain(amount);
        break;
      case PlayerUpgradeType.Knockback:
        pData.UpgradeKnockbackMultiplier(amount);
        break;
      case PlayerUpgradeType.Damage:
        pData.UpgradeDamageMultiplier(amount);
        break;
      case PlayerUpgradeType.WeaponSpeed:
        pData.UpgradeWeaponSpeedMultiplier(amount);
        break;
      case PlayerUpgradeType.WeaponCritChance:
        pData.UpgradeCriticalHitChanceMultiplier(amount);
        break;
      case PlayerUpgradeType.WeaponCritDamage:
        pData.UpgradeCriticalHitDamageMultiplier(amount);
        break;
      case PlayerUpgradeType.HealthRegeneration:
        pData.UpgradeHealthRegen(amount);
        break;
    }
  }
}

