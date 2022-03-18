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

  [Header("Speed")]
  [SerializeField] public float BaseSpeed = 1.0f;
  [SerializeField] public float Speed = 1.0f;

  [Header("HP")]
  [SerializeField] public float BaseHealth = 100;
  [SerializeField] public float MaxHealth = 100;
  public float CurrentHealth = 100;

  [Header("Exp")]
  [SerializeField] public float Experience = 0;
  [SerializeField] public float ExperienceMultiplier = 1.0f;

  public void ApplyCooldown(float amount)
  {
    CooldownMultiplier -= amount;
  }

  public void ApplyArea(float amount)
  {
    AreaMultiplier += amount;
  }

  public void ApplyDuration(float amount)
  {
    DurationMultiplier += amount;
  }

  public void IncreaseHealth(float amount)
  {
    float currentPercent = CurrentHealth / MaxHealth;
    MaxHealth += BaseHealth * amount;
    CurrentHealth = MaxHealth * currentPercent;
  }


  public void ApplySpeed(float amount)
  {
    Speed += BaseSpeed * amount;
  }

  public void ApplyExperienceUpgrade(float amount)
  {
    ExperienceMultiplier += ExperienceMultiplier * amount;
  }

  public static event Action<float> OnPlayerExperienceChangedAction;
  public void GainExp(float value)
  {
    Experience += value * ExperienceMultiplier;
    OnPlayerExperienceChangedAction?.Invoke(Experience);
  }

  public bool TakeDamage(float damage)
  {
    CurrentHealth -= damage;
    if (CurrentHealth < 0)
    {
      return true;
    }
    return false;
  }
}