using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
  private void OnEnable()
  {
    ResetUpgrade();
  }

  [Header("Debug")]
  [SerializeField] protected int currentUpgrade = 0;

  public abstract void ApplyUpgrade();

  public abstract bool CanBeUpgraded();

  public abstract string GetDisplayString();
  public virtual void ResetUpgrade()
  {
    currentUpgrade = 0;
  }
}
