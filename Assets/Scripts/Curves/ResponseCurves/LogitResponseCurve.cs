using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResponseCurves/Logit")]
public class LogitResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "Increasing",
    "Decreasing",
  };
  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>()
  {
    new ResponseCurveValues(0.07f, 1, 0.5f, 0, CurveType.Logit),
    new ResponseCurveValues(-0.07f, 1, 0.5f, 0, CurveType.Logit)
  };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(0.07f, 1, 0.5f, 0, CurveType.Logit);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampInput(x);
    return ClampValue(x, m * Mathf.Log((x / k - h) / (1 - (x / k - h))) + v);
  }

  protected float ClampValue(float x, float valueToClamp)
  {
    // needed because of vertical asymptotes of logit curves.
    if (float.IsNaN(valueToClamp))
    {
      // c is base "inflection"/ center point. (all curves "centered" at 0.5f by default) c adjusts it.
      // m is essentially the slope of the curve at that point 
      // so the sign of m * 1 or -1 gives 1 or -1 depending on m at the points before or after the center
      // logit curves also expand and contract, which changes the center point based on k... so...
      return Mathf.Clamp01(x > k * (h + 0.5f) ? Mathf.Sign(m) * 1 : Mathf.Sign(m) * -1);
    }
    return Mathf.Clamp01(valueToClamp);
  }
}
