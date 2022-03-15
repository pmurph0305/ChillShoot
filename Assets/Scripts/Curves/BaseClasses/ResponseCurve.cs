using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
// [CreateAssetMenu(fileName = "ResponseCurve", menuName = "3DR/ResponseCurve", order = 0)]
public abstract class ResponseCurve : ScriptableObject, IResponseCurve
{
  // protected List<string> PresetLabels;
  // protected List<ResponseCurveValues> PresetValues;
  // protected abstract List<string> PresetLabels { get; }
  // protected abstract List<ResponseCurveValues> Presets { get; }
  [SerializeField] bool OutputClamp01;
  [SerializeField] bool InputClamp01;

  [SerializeField] float OutputScale = 1.0f;

  [SerializeField]
  protected float m => values.m;
  [SerializeField]
  protected float k => values.k;
  [SerializeField]
  protected float v => values.v;
  [SerializeField]
  protected float h => values.h;
  [SerializeField]
  protected float r => values.r;

  [SerializeField]
  protected ResponseCurveValues values;

  public abstract float GetValue(float x);

  protected virtual float ClampOutput(float value)
  {
    if (!OutputClamp01)
    {
      return OutputScale * value;
    }
    return Mathf.Clamp01(OutputScale * value);
  }

  protected float ClampInput(float input)
  {
    if (!InputClamp01)
    {
      input = input % 1;
      return input;
    }
    return Mathf.Clamp01(input);
  }

}

