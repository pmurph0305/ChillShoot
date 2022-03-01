using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSpawnWeapon : PrefabSpawnWeapon
{
  [SerializeField] List<Transform> SpawnTransforms = new List<Transform>();
  int index = 0;
  protected override void Shoot()
  {
    // base.Shoot();
    pool.Get(SpawnTransforms[index].position);
    index++;
    if (index >= SpawnTransforms.Count)
    {
      index = 0;
    }
  }
}
