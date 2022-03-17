using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TransformSpawnInfo
{
  public Vector3 Position;
  public Quaternion Rotation;

  public Vector3 Scale;
  public TransformSpawnInfo(Vector3 position, Quaternion rotation, Vector3 scale)
  {
    this.Position = position;
    this.Rotation = rotation;
    this.Scale = scale;
  }

  public TransformSpawnInfo(Transform t)
  {
    this.Position = t.position;
    this.Rotation = t.rotation;
    this.Scale = t.localScale;
  }
}
