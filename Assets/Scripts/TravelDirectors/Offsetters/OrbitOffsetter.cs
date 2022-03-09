using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitOffsetter : TravelOffsetter
{
  Vector2 previous = Vector2.zero;
  Vector2 sincos = Vector2.zero;

  protected override Vector3 CalculateOffset()
  {
    sincos.x = Mathf.Sin(time);
    sincos.y = Mathf.Cos(time);
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
