using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ColorList
{
  [SerializeField] List<FloatColorPair> ColorPairs = new List<FloatColorPair>();


  [SerializeField] private FloatColorPair min;
  [SerializeField] private FloatColorPair max;
  public Color GetColorForValue(float value)
  {
    min = null;
    max = null;
    for (int i = 0; i < ColorPairs.Count; i++)
    {
      if (ColorPairs[i].Value >= value)
      {
        if (i == 0) { return ColorPairs[0].Color; }
        min = (i - 1 > -1) ? ColorPairs[i - 1] : ColorPairs[i];
        max = ColorPairs[i];
        break;
      }
    }
    if (min != null && max != null)
    {
      return Color.Lerp(min.Color, max.Color, (value - min.Value) / (max.Value - min.Value));
    }
    else
    {
      return ColorPairs[ColorPairs.Count - 1].Color;
    }
  }

  public Color GetEmissionForValue(float value)
  {
    min = null;
    max = null;
    for (int i = 0; i < ColorPairs.Count; i++)
    {
      if (ColorPairs[i].Value >= value)
      {
        if (i == 0) { return ColorPairs[0].Color; }
        min = (i - 1 > -1) ? ColorPairs[i - 1] : ColorPairs[i];
        max = ColorPairs[i];
        break;
      }
    }
    if (min != null && max != null)
    {
      return Color.Lerp(min._emission, max._emission, (value - min.Value) / (max.Value - min.Value));
    }
    else
    {
      return ColorPairs[ColorPairs.Count - 1]._emission;
    }
  }
  // public Vector4 GetEmission4ForValue(float value)
  // {
  //   for (int i = 0; i < ColorPairs.Count; i++)
  //   {
  //     if (ColorPairs[i].Value >= value)
  //     {
  //       if (i == 0) { return ColorPairs[0]._emission; }
  //       min = (i - 1 > -1) ? ColorPairs[i - 1] : ColorPairs[i];
  //       max = ColorPairs[i];
  //       break;
  //     }
  //   }
  //   if (min != null && max != null)
  //   {
  //     return Vector4.Lerp((Vector4)min._emission, (Vector4)max._emission, (value - min.Value) / (max.Value - min.Value));
  //   }
  //   else
  //   {
  //     return ColorPairs[ColorPairs.Count - 1]._emission;
  //   }
  // }
}




[System.Serializable]
public class FloatColorPair
{
  [SerializeField] Color color;
  public Color Color => color;

  [SerializeField, ColorUsageAttribute(false, true)] public Color _emission;

  // public Color Emission => _emission;

  [SerializeField] float value;
  public float Value => value;
}
