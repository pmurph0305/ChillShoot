using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveOffsetter : TravelOffsetter
{
  [SerializeField] AnimationCurve primaryCurve;
  [SerializeField] bool loopCurve;
  [SerializeField] protected float lengthOfCurve = 1.0f;
  float prev = 0f;
  [SerializeField, Tooltip("Multiplies the output of the response curve set by this value. As the curves are from 0->1, it's easier to adjust afterwards.")] float multiplier = 1.0f;
  protected override Vector3 CalculateOffset()
  {
    // float val = Curve.GetValue(time / lengthOfCurve);
    float val = primaryCurve.Evaluate(time / lengthOfCurve);
    float deltaS = val - prev;
    prev = val;
    return CombineWithDirection(deltaS);
  }



  protected override void AddTime(float deltaTime)
  {
    base.AddTime(deltaTime);
    if (loopCurve && time > lengthOfCurve)
    {
      time = time - lengthOfCurve;
    }
  }

  protected override void OnReset()
  {
    prev = 0f;
  }


}
