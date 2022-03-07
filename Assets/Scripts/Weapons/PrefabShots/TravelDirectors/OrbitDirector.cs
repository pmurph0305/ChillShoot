using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitDirector : MathDirector
{
  float time = 0.0f;
  [SerializeField] float TimeScale = 10f;
  Vector2 previous = Vector2.zero;
  Vector2 sincos = Vector2.zero;
  protected override Vector3 GetMathVector()
  {
    time += Time.deltaTime * TimeScale;
    sincos.x = Mathf.Sin(time);
    sincos.y = Mathf.Cos(time);
    Vector2 val = sincos - previous;
    previous = sincos;
    return val;
  }

  protected override Vector3 GetNewTravelDirection()
  {
    time = 0.0f;
    previous = Vector2.zero;
    return transform.up;
  }

}
