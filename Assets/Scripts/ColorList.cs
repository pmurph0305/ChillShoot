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
}

[System.Serializable]
public class FloatColorPair
{
  [SerializeField] Color color;
  public Color Color => color;
  [SerializeField] float value;
  public float Value => value;
}
