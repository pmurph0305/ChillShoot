using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformDirector : TravelDirector
{
  protected override Vector3 GetNewTravelDirection(WeaponInfo info)
  {
    return PlayerController.PlayerTransform.up;
  }
}
