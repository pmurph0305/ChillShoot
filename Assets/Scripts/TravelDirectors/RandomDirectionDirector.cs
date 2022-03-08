using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirectionDirector : TravelDirector
{
  protected override Vector3 GetNewTravelDirection()
  {
    return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
  }
}
