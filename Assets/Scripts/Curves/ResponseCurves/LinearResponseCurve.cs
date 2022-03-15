using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResponseCurves/Linear")]
[System.Serializable]
public class LinearResponseCurve : ResponseCurve
{

  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Increasing",
    "Decreasing",
  };

  [SerializeField, HideInInspector]
  protected List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>  {
      new ResponseCurveValues(1, 1, 0, 0, CurveType.Linear),
      new ResponseCurveValues(-1, 1, 1, 0, CurveType.Linear)
  };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(1, 1, 0, 0, CurveType.Linear);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    return ClampOutput(m * (x - h) + v);
  }
}
