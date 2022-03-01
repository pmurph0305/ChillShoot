using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformShot : PrefabShot
{
  [SerializeField] Transform t;
  protected override Vector3 GetTravelDirection()
  {
    return t.up;
  }
}
