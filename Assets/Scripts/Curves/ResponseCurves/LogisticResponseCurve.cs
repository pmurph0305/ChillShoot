using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ResponseCurves/Logistic")]
public class LogisticResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Increasing",
    "Decreasing",
    "Sharp FallUp",
    "Sharp FallDown"
  };

  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>()
  {
    new ResponseCurveValues(10, 1, 0, 0, CurveType.Logistic),
    new ResponseCurveValues(-10, 1, 0, 0, CurveType.Logistic),
    new ResponseCurveValues(50, 1, 0, 0, CurveType.Logistic),
    new ResponseCurveValues(-50, 1, 0, 0, CurveType.Logistic)
  };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(10, 1, 0, 0, CurveType.Logistic);
    }

  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    return ClampOutput(k * 1 / (1 + Mathf.Exp(-m * (x - h - 0.5f))) + v);
  }
}
