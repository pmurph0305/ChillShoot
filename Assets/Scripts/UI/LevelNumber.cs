using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class LevelNumber : MonoBehaviour
{
  [SerializeField] TMP_Text text;
  // public static Action<int> OnLevelUpAction;
  // Start is called before the first frame update
  void Awake()
  {
    LevelUp.OnLevelUpAction += OnLevelUpActionHandler;
  }

  void OnLevelUpActionHandler(int value)
  {
    text.text = value.ToString();
  }
}
