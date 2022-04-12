using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
  [SerializeField] List<VFXWeaponPoolEntry> VFXWeaponPoolEntries = new List<VFXWeaponPoolEntry>();

  Dictionary<WeaponKey, VFXPool> dict = new Dictionary<WeaponKey, VFXPool>();

  private void Awake()
  {
    PrefabShotBase.OnHitEnemyAction += OnHitEnemy;
    foreach (var item in VFXWeaponPoolEntries)
    {
      dict.Add(item.key, item.pool);
    }
  }

  private void OnDestroy()
  {
    PrefabShotBase.OnHitEnemyAction -= OnHitEnemy;
  }


  [SerializeField] LayerMask enemyMask;
  RaycastHit2D hit;
  void OnHitEnemy(ShotHitEventArgs e)
  {
    // Get(position);
    if (dict.ContainsKey(e.key))
    {
      if (dict[e.key].RaycastForPosition)
      {
        hit = Physics2D.Raycast(e.ShotPosition, e.EnemyPosition - e.ShotPosition, 1f, enemyMask.value);
        e.EnemyPosition = hit.point;
      }
      if (dict[e.key].RotateToDirection)
      {
        Quaternion r = Quaternion.LookRotation(Vector3.forward, e.ShotTravelDirection);
        dict[e.key].Get(e.EnemyPosition, r);
      }
      else
      {
        dict[e.key].Get(e.EnemyPosition);
      }
    }
  }
}

[System.Serializable]
public class VFXWeaponPoolEntry
{
  [SerializeField] public WeaponKey key;
  [SerializeField] public VFXPool pool;
}