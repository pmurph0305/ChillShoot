using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResponseCurves/Step")]
public class StepResponseCurve : ResponseCurve
{

  [SerializeField, HideInInspector]
  private List<string> presetLabels = new List<string>()
  {
    "Start 0.25 Wide",
    "Center 0.25 Wide",
    "End 0.25 Wide",
    "2 Step Curve"
  };

  [SerializeField, HideInInspector]
  protected List<ResponseCurveValues> presetValues = new List<ResponseCurveValues>  {
    new ResponseCurveValues(1, 0f, 0.25f, 1, CurveType.Step,1 ),
    new ResponseCurveValues(1, 0.375f, 0.625f, 1, CurveType.Step,1),
    new ResponseCurveValues(1, 0.75f, 1f, 1, CurveType.Step,1),
    new ResponseCurveValues(1, 0.2f, 0.3f, 0.7f, CurveType.Step,0.8f),
  };


  void OnEnable()
  {
    if (values == null)
    {
      values = new ResponseCurveValues(1, 0f, 0.25f, 1, CurveType.Step, 1);
    }
  }

  public override float GetValue(float x)
  {
    if (x > k && x < v) { return m; }
    if (x > h && x < r) { return m; }
    return 0;
  }
}