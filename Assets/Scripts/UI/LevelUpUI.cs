using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelUpUI : MonoBehaviour
{
  public static Action OnLevelUpAction;

  private void Awake()
  {
    this.gameObject.SetActive(false);
    OnLevelUpAction += OnLevelUpActionHandler;
  }

  void OnLevelUpActionHandler()
  {
    Time.timeScale = 0.0f;
    this.gameObject.SetActive(true);
  }

  public void OnLevelUpButtonClicked()
  {
    Time.timeScale = 1.0f;
    this.gameObject.SetActive(false);
  }

}
