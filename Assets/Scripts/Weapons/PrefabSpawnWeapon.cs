using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PrefabSpawnWeapon : Weapon
{
  [SerializeField] protected PrefabPool<PrefabShot> pool;
  protected override void Shoot()
  {
    // shots automatically update position when gotten from pool.
    PrefabShot s = pool.Get();
  }
}
