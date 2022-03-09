using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitOffsetter : TravelOffsetter
{
  float time = 0.0f;
  Vector2 previous = Vector2.zero;
  Vector2 sincos = Vector2.zero;

  const float halfPI = 1.57079633f;
  //todo: doesn't actually start from 0,0.
  public override Vector3 GetOffset(float deltaTime)
  {
    time += deltaTime * timeScale;
    sincos.x = Mathf.Sin(time);
    sincos.y = Mathf.Cos(time);
    Vector2 val = sincos - previous;
    previous = sincos;
    return val;
  }

  protected override void OnReset()
  {
    time = 0.0f;
    previous = Vector2.zero;
    sincos = Vector2.zero;
  }
}
