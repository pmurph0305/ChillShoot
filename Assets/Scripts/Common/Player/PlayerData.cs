using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class PlayerData
{
  [Header("Weapons")]
  public float CooldownMultiplier = 1.0f;
  public float AreaMultiplier = 1.0f;
  public float DurationMultiplier = 1.0f;
  public float KnockbackMultiplier = 1.0f;
  public float DamageMultiplier = 1.0f;
  public float WeaponSpeedMultiplier = 1.0f;
  public float CriticalHitDamageMultiplier = 1.0f;
  public float CriticalHitChanceMultiplier = 1.0f;

  [Header("Speed")]
  [SerializeField] public float BaseSpeed = 3.0f;
  [SerializeField] public float Speed = 3.0f;

  [Header("HP")]
  [SerializeField] public float BaseHealth = 100;
  [SerializeField] public float MaxHealth = 100;
  [SerializeField] public float HealthRegen = 0.0f;

  [Header("Exp")]
  [SerializeField] public float Experience = 0;
  [SerializeField] public float ExperienceMultiplier = 1.0f;

  #region weapons
  public void UpgradeCooldownMultiplier(float amount)
  {
    CooldownMultiplier -= amount;
  }

  public void UpgradeAreaMultiplier(float amount)
  {
    AreaMultiplier += amount;
  }

  public void UpgradeDurationMultiplier(float amount)
  {
    DurationMultiplier += amount;
  }

  public void UpgradeKnockbackMultiplier(float amount)
  {
    KnockbackMultiplier += amount;
  }

  public void UpgradeDamageMultiplier(float amount)
  {
    DamageMultiplier += amount;
  }

  public void UpgradeWeaponSpeedMultiplier(float amount)
  {
    WeaponSpeedMultiplier += amount;
  }

  public void UpgradeCriticalHitDamageMultiplier(float amount)
  {
    CriticalHitDamageMultiplier += amount;
  }

  public void UpgradeCriticalHitChanceMultiplier(float amount)
  {
    CriticalHitChanceMultiplier += amount;
  }
  #endregion

  public void UpgradeMaxHealth(float amount)
  {
    MaxHealth += BaseHealth * amount;
  }

  public void UpgradeHealthRegen(float amount)
  {
    HealthRegen += amount;
  }
  public void UpgradeMovementSpeed(float amount)
  {
    Speed += BaseSpeed * amount;
  }

  public void UpgradeExperienceGain(float amount)
  {
    ExperienceMultiplier += ExperienceMultiplier * amount;
  }

  public static event Action<float> OnPlayerExperienceChangedAction;
  public void GainExp(float value)
  {
    Experience += value * ExperienceMultiplier;
    OnPlayerExperienceChangedAction?.Invoke(Experience);
  }


}