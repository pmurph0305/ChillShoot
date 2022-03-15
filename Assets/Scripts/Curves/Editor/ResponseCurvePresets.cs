using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResponseCurvePresets
{
  public static Dictionary<ResponseCurveValues, string> GetPresets(ResponseCurve c)
  {
    return new Dictionary<ResponseCurveValues, string>();
  }

  private static List<string> linearLabels = new List<string>() {
    "Increasing",
    "Decreasing",
  };

}
