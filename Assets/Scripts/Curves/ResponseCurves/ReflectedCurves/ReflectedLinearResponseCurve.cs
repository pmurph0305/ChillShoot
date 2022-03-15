using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ResponseCurves/Reflected/Linear")]
public class ReflectedLinearResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>()
  {
        new ResponseCurveValues(2, 1, 0.5f, 0.5f, CurveType.ReflectedLinear),
        new ResponseCurveValues(-2, 1, -0.5f, 0.5f, CurveType.ReflectedLinear),
  };

  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Increasing",
    "Decreasing",
  };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(2, 1, 0.5f, 0.5f, CurveType.ReflectedLinear);
    }
  }

  public override float GetValue(float x)
  {
    // instead of using r, using h as the reflection point makes it much more intuitive to use.
    // and as the slope is constant, we can do this without impacting how it can be used.
    if (x <= h)
    {
      return ClampOutput(m * Mathf.Pow((x - h), 1) + v + 0.5f);
    }
    else
    {
      return GetValue(h - (x - h));
    }
  }
}
