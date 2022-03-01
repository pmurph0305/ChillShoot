using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransformSpawnInfo
{
  public Vector3 Position;
  public Quaternion Rotation;

  public TransformSpawnInfo(Vector3 position, Quaternion rotation)
  {
    this.Position = position;
    this.Rotation = rotation;
  }
}
