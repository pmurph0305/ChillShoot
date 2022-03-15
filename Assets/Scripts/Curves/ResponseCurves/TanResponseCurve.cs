using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ResponseCurves/Tan")]
public class TanResponseCurve : ResponseCurve
{
  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>() {
    "1 Cycle",
  };

  [SerializeField, HideInInspector]
  private List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>() {
    new ResponseCurveValues(1, 1, 0, 0, CurveType.Tan)
  };

  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(1, 1, 0, 0, CurveType.Tan);
    }
  }

  public override float GetValue(float x)
  {
    x = ClampOutput(x);
    // desenseitivity to k, and shift x and y 0.5 so it's more in the range by default.
    // also freqency /2 as we really just want one.. cycle in 0-1
    return ClampOutput(0.01f * k * Mathf.Tan(m / 2 * (x - h + 0.5f) * 2 * Mathf.PI) + v + 0.5f);
  }
}
