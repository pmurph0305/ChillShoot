using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ResponseCurves/Exponential")]
public class ExponentialResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Increasing",
    "Decreasing",
  };

  [SerializeField, HideInInspector]
  protected List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>  {
      new ResponseCurveValues(8, 1, 0, 1, CurveType.Exponential),
      new ResponseCurveValues(-8, 1, 0, 0, CurveType.Exponential)
    };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(8, 1, 0, 1, CurveType.Exponential);
    }
    Debug.Log(values == null);
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    return ClampValue(k * Mathf.Exp(m * (x - h)) + v);
  }

  protected float ClampValue(float val)
  {
    val = Mathf.Clamp01(val);
    if (float.IsNaN(val))
    {
      return 1;
    }
    return val;
  }
}
