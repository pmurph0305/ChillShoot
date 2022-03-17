using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelUpUI : MonoBehaviour
{

  private void Awake()
  {
    this.gameObject.SetActive(false);
    LevelUp.OnLevelUpUpgradeAction += OnLevelUpUpgradeActionHandler;
  }

  [SerializeField] RectTransform buttonParent;
  [SerializeField] GameObject buttonPrefab;
  List<GameObject> CreatedButtons = new List<GameObject>();
  void OnLevelUpUpgradeActionHandler(List<Upgrade> ups)
  {
    Time.timeScale = 0.0f;
    foreach (var upgrade in ups)
    {
      GameObject o = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, buttonParent);
      UpgradeButton ub = o.GetComponent<UpgradeButton>();
      ub.SetUpgrade(upgrade, this);
      CreatedButtons.Add(o);
    }
    this.gameObject.SetActive(true);
  }


  public static event Action<Upgrade> OnUpgradeChosenAction;
  public void OnLevelUpButtonClicked(Upgrade upgrade)
  {
    Time.timeScale = 1.0f;
    foreach (var go in CreatedButtons)
    {
      Destroy(go);
    }
    upgrade.ApplyUpgrade();
    OnUpgradeChosenAction?.Invoke(upgrade);
    this.gameObject.SetActive(false);
  }

}
