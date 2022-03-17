using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelUp : MonoBehaviour
{
  [SerializeField] UpgradeList upgradeList;

  [SerializeField] float MaxLevelExp = 10000;
  [SerializeField] float StartExp = 100;
  [SerializeField] int MaxLevel = 100;
  [SerializeField] int CurrentLevel = 1;
  [SerializeField] float NextLevelExp;
  float previousLevelExp = 0f;

  [SerializeField] AnimationCurve ExpCurve;

  public static event Action<float> OnPercentLevelChangedAction;
  public static event Action<int> OnLevelUpAction;
  public static event Action<List<Upgrade>> OnLevelUpUpgradeAction;
  private void Awake()
  {
    // OnExperienceChanged += OnExperienceChangedHandler;
    PlayerData.OnPlayerExperienceChangedAction += OnPlayerExperienceChangedHandler;
    NextLevelExp = GetNextLevelExp(0);
  }

  private void Start()
  {
    OnPercentLevelChangedAction?.Invoke(0);
    OnLevelUpAction?.Invoke(0);
  }

  [SerializeField] float evaluatedValue;
  float GetNextLevelExp(int CurrentLevel)
  {
    previousLevelExp = NextLevelExp;
    float percentMax = (float)CurrentLevel / (float)MaxLevel;
    evaluatedValue = ExpCurve.Evaluate(percentMax);
    return NextLevelExp + StartExp + MaxLevelExp * ExpCurve.Evaluate(percentMax);
  }

  void OnPlayerExperienceChangedHandler(float value)
  {
    if (value > NextLevelExp)
    {
      // level up.
      IncreaseCurrentLevel();
    }
    OnPercentLevelChangedAction?.Invoke((value - previousLevelExp) / (NextLevelExp - previousLevelExp));
  }

  void IncreaseCurrentLevel()
  {
    CurrentLevel++;
    OnLevelUpAction?.Invoke(CurrentLevel);
    NextLevelExp = GetNextLevelExp(CurrentLevel);
    List<Upgrade> ups = upgradeList.GetUpgradesToDisplay();
    OnLevelUpUpgradeAction?.Invoke(ups);
  }

}
