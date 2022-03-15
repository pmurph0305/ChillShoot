using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResponseCurveValues
{
  //NOTE: For setting the labels for values, see ResponseCurveValuesPropertyDrawer
  // I did not want to clutter the actual Curve class, or this class with information that's only useful in the editor
  // so they are all set in the ResponeCurveValuesPropertyDrawer.cs file.

  /// <summary>
  /// Creates a set of response curve values.
  /// </summary>
  /// <param name="m"></param>
  /// <param name="k"></param>
  /// <param name="v"></param>
  /// <param name="h"></param>
  /// <param name="curve"> curve these values are from</param>
  /// <param name="r">optional reflection point parameter</param>
  public ResponseCurveValues(float m, float k, float v, float h, CurveType curveType, float r = 0.0f)
  {
    this.curveType = curveType;
    this.m = m;
    this.k = k;
    this.v = v;
    this.h = h;
    this.r = r;
  }
  [SerializeField]
  public CurveType curveType;
  // [SerializeField]
  // public LinearResponseCurve curve;
  [SerializeField]
  public float m;
  [SerializeField]
  public float k;
  [SerializeField]
  public float v;
  [SerializeField]
  public float h;
  [SerializeField]
  public float r;
}