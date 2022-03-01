using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSpawnWeapon : PrefabSpawnWeapon
{
  [SerializeField] List<Transform> SpawnTransforms = new List<Transform>();
  int index = 0;

  Vector3 spawnPos;

  protected override void Shoot()
  {
    base.Shoot();
    index++;
    if (index >= SpawnTransforms.Count)
    {
      index = 0;
    }
  }

  protected override Vector3 GetShotSpawnPosition()
  {
    return SpawnTransforms[index].position;
  }

  protected override Quaternion GetShotSpawnRotation()
  {
    return SpawnTransforms[index].rotation;
  }

}
