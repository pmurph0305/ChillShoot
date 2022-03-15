using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitOffsetter : MultiAxisOffsetter
{
  Vector2 previous = Vector2.zero;
  Vector2 sincos = Vector2.zero;

  protected override Vector3 CalculateOffset()
  {
    sincos.x = Mathf.Sin(time + Mathf.PI);
    sincos.y = Mathf.Cos(time + Mathf.PI);
    Vector2 val = sincos - previous;
    previous = sincos;
    return primaryAxis * val.x + secondaryAxis * val.y;
    // return val;
  }

  protected override void OnResetOffset()
  {
    // so that secondary axis get's recalculated.
    base.OnResetOffset();
    previous = Vector2.zero;
    sincos = Vector2.zero;
  }
}
