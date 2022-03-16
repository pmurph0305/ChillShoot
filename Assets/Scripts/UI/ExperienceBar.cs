using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ExperienceBar : MonoBehaviour
{
  public static Action<float> OnPercentLevelChangedAction;
  [SerializeField] Image image;
  // Start is called before the first frame update
  void Awake()
  {
    OnPercentLevelChangedAction += OnPercentLevelChangedActionHandler;
  }

  void OnPercentLevelChangedActionHandler(float percent)
  {
    image.fillAmount = percent;
  }
}
