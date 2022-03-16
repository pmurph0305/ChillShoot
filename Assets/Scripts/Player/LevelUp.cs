using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelUp : MonoBehaviour
{

  public static Action<float> OnExperienceChanged;

  [SerializeField] float MaxLevelExp = 10000;
  [SerializeField] float StartExp = 100;
  [SerializeField] int MaxLevel = 100;
  [SerializeField] int CurrentLevel = 1;
  [SerializeField] float NextLevelExp;
  float previousLevelExp = 0f;

  [SerializeField] AnimationCurve ExpCurve;

  private void Awake()
  {
    OnExperienceChanged += OnExperienceChangedHandler;
    NextLevelExp = GetNextLevelExp(0);
  }

  private void Start()
  {
    ExperienceBar.OnPercentLevelChangedAction.Invoke(0);
    LevelNumber.OnLevelUpAction.Invoke(0);
  }

  [SerializeField] float evaluatedValue;
  float GetNextLevelExp(int CurrentLevel)
  {
    previousLevelExp = NextLevelExp;
    float percentMax = (float)CurrentLevel / (float)MaxLevel;
    evaluatedValue = ExpCurve.Evaluate(percentMax);
    return NextLevelExp + StartExp + MaxLevelExp * ExpCurve.Evaluate(percentMax);
  }

  void OnExperienceChangedHandler(float value)
  {
    if (value > NextLevelExp)
    {
      // level up.
      IncreaseCurrentLevel();
    }
    ExperienceBar.OnPercentLevelChangedAction.Invoke((value - previousLevelExp) / (NextLevelExp - previousLevelExp));
  }

  void IncreaseCurrentLevel()
  {
    CurrentLevel++;
    LevelNumber.OnLevelUpAction.Invoke(CurrentLevel);
    NextLevelExp = GetNextLevelExp(CurrentLevel);
    LevelUpUI.OnLevelUpAction.Invoke();
  }

}
