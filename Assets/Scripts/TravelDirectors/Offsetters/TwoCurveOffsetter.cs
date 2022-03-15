using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoCurveOffsetter : CurveOffsetter
{
  [Header("Secondary Curve")]
  [SerializeField] AnimationCurve secondaryCurve;
  [SerializeField] protected Direction secondaryTransformDirection = Direction.Up;
  protected Vector3 secondaryAxis;
  protected float secondaryPreviousValue;

  protected override Vector3 CalculateOffset()
  {
    return base.CalculateOffset() + CombineWithDirection(EvaluateCurve(secondaryCurve, ref secondaryPreviousValue), secondaryAxis);
  }

  protected override void OnResetOffset()
  {
    base.OnResetOffset();
    secondaryAxis = GetTransformDirection(secondaryTransformDirection);
    secondaryPreviousValue = 0.0f;
  }

  protected override void OnLoopCurve()
  {
    base.OnLoopCurve();
    secondaryPreviousValue = secondaryCurve.Evaluate(0);
  }
}
