using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateOffsetter : DirectionOffsetter
{

  [SerializeField] float frequency = 10f;
  [SerializeField] float amplitude = 1f;

  Vector3 oscillation = Vector3.zero;
  float previousS;

  protected override Vector3 CalculateOffset()
  {
    float s = amplitude * Mathf.Sin(time * frequency);
    // because otherwise, we're continually adding s, and s increases, we'd be doing it additively
    // by subtracting the previous s, we're using the actual change in S this frame.
    // which is the oscilation we want to add.
    float deltaS = s - previousS;
    previousS = s;
    return CombineWithDirection(deltaS);
  }

  protected override void OnReset()
  {
    previousS = 0.0f;
  }
}
