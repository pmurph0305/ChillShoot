using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponShot
{
  /// <summary>
  /// Called by Weapon info when a shot hits an enemy.
  /// </summary>
  void OnHitEnemy(EnemyControllerBase enemy);
}
