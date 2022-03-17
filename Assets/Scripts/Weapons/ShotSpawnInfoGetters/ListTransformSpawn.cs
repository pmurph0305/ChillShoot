using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTransformSpawn : ShotSpawnInfoGetter
{
  [SerializeField] List<Transform> SpawnTransforms = new List<Transform>();
  int index = 0;

  [SerializeField] List<Transform> UnlockableTransforms = new List<Transform>();

  protected override Vector3 GetSpawnPosition()
  {
    return SpawnTransforms[index].position;
  }

  protected override Quaternion GetSpawnRotation()
  {
    return SpawnTransforms[index].rotation;
  }



  TransformSpawnInfo info;
  public override TransformSpawnInfo GetTransformSpawnInfo()
  {
    info = new TransformSpawnInfo(GetSpawnPosition(), GetSpawnRotation(), GetScale());
    index++;
    if (index >= SpawnTransforms.Count)
    {
      index = 0;
    }
    return info;
  }

  public override int GetNumberOfSpawnLocations()
  {
    return SpawnTransforms.Count;
  }
}
