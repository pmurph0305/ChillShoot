using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ExperienceBar : MonoBehaviour
{
  [SerializeField] Image image;
  // Start is called before the first frame update
  void Awake()
  {
    LevelUp.OnPercentLevelChangedAction += OnPercentLevelChangedActionHandler;
  }

  void OnPercentLevelChangedActionHandler(float percent)
  {
    image.fillAmount = percent;
  }
}
