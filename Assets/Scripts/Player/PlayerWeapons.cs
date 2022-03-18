using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
  [SerializeField] List<PlayerWeaponData> PlayerWeaponData;

  private void Awake()
  {
    WeaponUpgrade.OnPlayerAcquireNewWeaponAction += OnPlayerAcquireNewWeaponActionHandler;
  }

  void OnPlayerAcquireNewWeaponActionHandler(WeaponUpgrade upgrade)
  {
    foreach (var item in PlayerWeaponData)
    {
      if (item.key == upgrade.weaponKey)
      {
        item.gameObject.SetActive(true);
      }
    }
  }
}
[System.Serializable]
public class PlayerWeaponData
{
  public GameObject gameObject;
  public WeaponKey key;
}