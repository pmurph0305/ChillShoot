using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
public static class Vector3Extensions
{
  //Union used by InvSqrt
  [StructLayout(LayoutKind.Explicit)]
  struct FloatIntUnion
  {
    [FieldOffset(0)]
    public float x;

    [FieldOffset(0)]
    public int i;
  }

  static FloatIntUnion union = new FloatIntUnion();

  static float InvSqrt(float x)
  {
    float x2 = x * 0.5f;
    union.x = x;
    union.i = 0x5f3759df - (union.i >> 1);
    x = union.x;
    x = x * (1.5f - x2 * x * x);
    return x;
  }

  public static Vector3 FastNormalized(this Vector3 src)
  {
    float inversedMagnitude = InvSqrt(src.sqrMagnitude);
    return src * inversedMagnitude;
  }


}
