using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResponseCurves/Polynomial")]
public class PolynomialResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "2 Increasing",
    "2 Decreasing",
    "4 Increasing",
    "4 Decreasing",
  };

  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>()
      {
        new ResponseCurveValues(1, 2, 0, 0, CurveType.Polynomial),
        new ResponseCurveValues(-1, 2, 1, 0, CurveType.Polynomial),
        new ResponseCurveValues(1, 4, 0, 0, CurveType.Polynomial),
        new ResponseCurveValues(-1, 4, 1, 0, CurveType.Polynomial)
    };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(1, 2, 0, 0, CurveType.Polynomial);
    }

  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    return ClampOutput(m * Mathf.Pow(Mathf.Abs(x - h), k) + v);
  }
}

