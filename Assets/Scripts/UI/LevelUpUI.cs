using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelUpUI : MonoBehaviour
{
  public static Action<List<Upgrade>> OnLevelUpAction;

  private void Awake()
  {
    this.gameObject.SetActive(false);
    OnLevelUpAction += OnLevelUpActionHandler;
  }

  [SerializeField] RectTransform buttonParent;
  [SerializeField] GameObject buttonPrefab;
  List<GameObject> CreatedButtons = new List<GameObject>();
  void OnLevelUpActionHandler(List<Upgrade> ups)
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

  public void OnLevelUpButtonClicked(Upgrade upgrade)
  {
    Time.timeScale = 1.0f;
    foreach (var go in CreatedButtons)
    {
      Destroy(go);
    }
    upgrade.ApplyUpgrade();
    UpgradeList.OnUpgradeChosenAction.Invoke(upgrade);
    this.gameObject.SetActive(false);
  }

}
