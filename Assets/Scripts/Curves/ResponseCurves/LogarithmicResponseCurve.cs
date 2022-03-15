using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ResponseCurves/Logarithmic")]

public class LogarithmicResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Increasing",
    "Decreasing",
    "Increasing Sharper",
    "Decreasing Sharper,"
  };

  [SerializeField, HideInInspector]
  protected List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>  {
      new ResponseCurveValues(1, 1.44f, 0, 0,  CurveType.Logarithmic),
      new ResponseCurveValues(1, -1.44f, 1, 0,  CurveType.Logarithmic),
      new ResponseCurveValues(54, 0.25f, 0, 0,  CurveType.Logarithmic),
       new ResponseCurveValues(54, -0.25f, 1, 0,  CurveType.Logarithmic)
    };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(1, 1.44f, 0, 0, CurveType.Logarithmic);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    // x+1 to more natrually shift to zero.
    return ClampOutput(k * Mathf.Log(m * x + 1 - h) + v);
  }
}
