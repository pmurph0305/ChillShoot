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
  [SerializeField] protected Transform ShotSpawnTransform;
  [SerializeField] protected PrefabPool<PrefabShot> pool;

  protected override Vector3 GetShotSpawnPosition()
  {
    return ShotSpawnTransform.position;
  }

  protected override Quaternion GetShotSpawnRotation()
  {
    return ShotSpawnTransform.rotation;
  }

  protected override void Shoot()
  {
    // shots automatically update position when gotten from pool.
    PrefabShot s = pool.Get(GetShotSpawnPosition(), GetShotSpawnRotation());
  }
}
