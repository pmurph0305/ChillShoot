using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTransformSpawn : ShotSpawnInfoGetter
{
  [SerializeField] Transform SpawnTransform;

  public override TransformSpawnInfo GetTransformSpawnInfo()
  {
    return new TransformSpawnInfo(GetSpawnPosition(), GetSpawnRotation());
  }

  protected override Vector3 GetSpawnPosition()
  {
    return SpawnTransform.position;
  }

  protected override Quaternion GetSpawnRotation()
  {
    return SpawnTransform.rotation;
  }


}
