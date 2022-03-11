using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitOffsetter : TravelOffsetter
{
  Vector2 previous = Vector2.zero;
  Vector2 sincos = Vector2.zero;

  const float halfPi = 1.57079632679f;
  const float pi = 3.14159265359f;
  protected override Vector3 CalculateOffset()
  {
    sincos.x = Mathf.Sin(time + Mathf.PI);
    sincos.y = Mathf.Cos(time + Mathf.PI);
    Vector2 val = sincos - previous;
    previous = sincos;
    return transform.right * val.x + transform.up * val.y;
    // return val;
  }


  protected override void OnReset()
  {
    previous = Vector2.zero;
    sincos = Vector2.zero;
  }
}
