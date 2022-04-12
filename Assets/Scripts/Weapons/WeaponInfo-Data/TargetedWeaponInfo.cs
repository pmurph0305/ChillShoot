using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class TargetedWeaponInfo : WeaponInfo
{
  [Header("Targeted Weapon info")]
  [SerializeField] public Transform weaponTransform;
  [SerializeField] public Transform target;
  [SerializeField] LayerMask TargetLayerMask;

  EnemyControllerBase trackedEnemy;

  public virtual void UpdateTarget()
  {
    if (weaponTransform == null) { Debug.LogWarning("Null weapon transform"); return; }
    List<Collider2D> cols = Physics2D.OverlapCircleAll(weaponTransform.position, 10f, TargetLayerMask).ToList();
    if (cols.Count > 0)
    {
      cols.Sort((a, b) =>
      {
        float da = Vector3.SqrMagnitude(weaponTransform.position - a.transform.position);
        float db = Vector3.SqrMagnitude(weaponTransform.position - b.transform.position);
        if (da < db) return -1;
        if (da > db) return 1;
        return 0;
      });

      foreach (var item in cols)
      {
        if (EnemyDictionary.ContainsActive(item.transform))
        {
          target = item.transform;
          trackedEnemy = EnemyDictionary.GetActive(item.transform);
          trackedEnemy.OnEnemyReleased += OnTargetReleasedHandler;
          break;
        }
      }
    }
  }

  public bool NeedsUpdate()
  {
    return trackedEnemy == null || !trackedEnemy.isActiveAndEnabled;
  }

  public void OnTargetReleasedHandler(EnemyControllerBase enemyController)
  {
    trackedEnemy.OnEnemyReleased -= OnTargetReleasedHandler;
    UpdateTarget();
  }
}
