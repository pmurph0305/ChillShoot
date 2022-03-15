using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ResponseCurves/Sin")]
public class SinResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "1 Cycle",
  };

  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>() {
    new ResponseCurveValues(1, 1, 0, 0, CurveType.Sin)
  };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(1, 1, 0, 0, CurveType.Sin);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    // we want the base response to be within 0 to 1 x and y.
    return ClampOutput(0.5f * k * Mathf.Sin(m * (x - h) * Mathf.PI * 2) + v + 0.5f);
  }
}
