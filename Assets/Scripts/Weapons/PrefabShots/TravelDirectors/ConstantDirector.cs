using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDirector : TravelDirector
{
  protected override Vector3 GetNewTravelDirection(WeaponInfo info)
  {
    return travelDirection;
  }
}
