using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveOffsetter : TravelOffsetter
{
  [Header("Curve Offsetter")]
  [SerializeField, Tooltip("A constant horizontal curve will have no effect, as the change in the graph is used.")] protected AnimationCurve primaryCurve;
  [SerializeField] protected bool loopCurve;
  [SerializeField] protected float lengthOfCurve = 1.0f;
  float primaryPreviousValue = 0f;
  [SerializeField, Tooltip("Multiplies the output of the response curve set by this value. As the curves are from 0->1, it's easier to adjust afterwards.")] float multiplier = 1.0f;
  protected override Vector3 CalculateOffset()
  {
    return CombineWithDirection(EvaluateCurve(primaryCurve, ref primaryPreviousValue));
  }

  protected virtual float EvaluateCurve(AnimationCurve curve, ref float prev)
  {
    float val = curve.Evaluate(time / lengthOfCurve) * multiplier;
    float deltaS = val - prev;
    prev = val;
    return deltaS;
  }

  protected override void AddTime(float deltaTime)
  {
    base.AddTime(deltaTime);
    if (loopCurve && time > lengthOfCurve)
    {
      time = time - lengthOfCurve;
      OnLoopCurve();
    }
  }

  protected virtual void OnLoopCurve()
  {
    primaryPreviousValue = primaryCurve.Evaluate(0);
  }

  protected override void OnResetOffset()
  {
    primaryPreviousValue = 0f;
  }


}
