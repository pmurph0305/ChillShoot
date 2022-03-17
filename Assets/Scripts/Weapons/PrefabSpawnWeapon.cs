using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Prefab spawn weapons are reponsible for getting a PrefabShot from the pool, and setting it's position and rotation on spawn.
/// </summary>
public class PrefabSpawnWeapon : Weapon
{
  //TODO: seperate component to get the transform to spawn at.
  // [SerializeField] protected Transform ShotSpawnTransform; 
  [SerializeField] protected PrefabPool<PrefabShot> pool;

  protected override void Shoot()
  {
    for (int i = 0; i < weaponInfo.NumberOfShots; i++)
    {
      // get position and rotation for where to spawn the shot.
      PrefabShot s = pool.Get(weaponInfo.GetTransformSpawnInfo());
      // Debug.Log("Shoot:" + i, s.gameObject);
    }
  }
}
