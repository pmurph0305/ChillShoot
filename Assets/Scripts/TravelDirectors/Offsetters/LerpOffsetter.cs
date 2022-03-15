using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpOffsetter : TravelOffsetter
{
  [Header("Lerp Offsetter")]
  [SerializeField] float lerpTime = 1f;
  [SerializeField] float lerpAmount = 1f;

  bool toFrom = false;
  float val;
  float previous;
  protected override Vector3 CalculateOffset()
  {
    if (time > lerpTime)
    {
      toFrom = !toFrom;
      time -= lerpTime;
    }
    if (!toFrom)
    {
      val = Mathf.Lerp(-lerpAmount, lerpAmount, time / lerpTime);
    }
    else
    {
      val = Mathf.Lerp(lerpAmount, -lerpAmount, time / lerpTime);
    }
    float change = val - previous;
    previous = val;
    return CombineWithDirection(change);
  }

  protected override void OnResetOffset()
  {
    // because we want to start at the "halfway" point in time.
    time = 0.5f;
    previous = 0.0f;
    toFrom = false;
  }

}
