using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResponseCurves/Gaussian")]
public class GaussianResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Center Wide",
    "Center Narrow",
  };

  [SerializeField, HideInInspector]
  protected List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>() {
      new ResponseCurveValues(0.25f, 1.175f, -0.175f, 0.5f, CurveType.Gaussian),
      new ResponseCurveValues(0.1f, 1, 0, 0.5f, CurveType.Gaussian)
    };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(0.1f, 1, 0, 0.5f, CurveType.Gaussian);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    return ClampOutput(k * Mathf.Exp(-1 * Mathf.Pow((x - h), 2) / (2 * Mathf.Pow(m, 2))) + v);
  }
}
