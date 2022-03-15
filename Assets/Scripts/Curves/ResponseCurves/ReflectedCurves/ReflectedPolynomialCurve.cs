using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResponseCurves/Reflected/Polynomial")]
public class ReflectedPolynomialCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
      "Increasing Peak",
      "Decreasing Peak",
      "Increasing Hump",
      "Decreasing Hump",
    };

  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>()
      {
        new ResponseCurveValues(4, 2, 0, 0, CurveType.ReflectedPolynomial, 0.5f),
        new ResponseCurveValues(-4, 2, 1, 0, CurveType.ReflectedPolynomial, 0.5f),
        new ResponseCurveValues(-4, 2, 1, 0.5f, CurveType.ReflectedPolynomial, 0.5f),
        new ResponseCurveValues(4, 2, 0, 0.5f, CurveType.ReflectedPolynomial, 0.5f),
    };


  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(4, 2, 0, 0, CurveType.ReflectedPolynomial, 0.5f);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    if (x <= r)
    {
      return ClampOutput(m * Mathf.Pow(Mathf.Abs(x - h), k) + v);
    }
    else
    {
      // return value from before the reflected point.
      return GetValue(r - (x - r));
    }
  }
}
