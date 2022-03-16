using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
  [SerializeField] TMP_Text buttonText;

  [SerializeField] Button button;

  public void SetUpgrade(Upgrade u, LevelUpUI ui)
  {
    buttonText.text = u.GetDisplayString();
    button.onClick.AddListener(() => OnClick(u, ui));
  }

  void OnClick(Upgrade u, LevelUpUI ui)
  {
    ui.OnLevelUpButtonClicked(u);
  }
}
