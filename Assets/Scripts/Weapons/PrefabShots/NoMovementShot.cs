using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMovementShot : PrefabShot
{
  protected override Vector3 GetTravelDirection()
  {
    return Vector3.zero;
  }
}
