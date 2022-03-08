using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateDirector : MathDirector
{
  float time = 0.0f;
  Vector3 oscillation = Vector3.zero;
  [SerializeField] float frequency = 10f;
  [SerializeField] float amplitude = 1f;
  [SerializeField] float phaseShift = 1f;
  protected override Vector3 GetNewTravelDirection()
  {
    time = 0.0f;
    previousS = 0.0f;
    return transform.up;
  }

  float previousS;
  protected override Vector3 GetMathVector()
  {
    time += Time.deltaTime;
    float s = amplitude * Mathf.Sin(time * frequency + phaseShift);
    // because otherwise, we're continually adding s, and s increases, we'd be doing it additively
    // by subtracting the previous s, we're using the actual change in S this frame.
    // which is the oscilation we want to add.
    float deltaS = s - previousS;
    previousS = s;
    oscillation = Vector3.zero;
    switch (direction)
    {
      case Direction.Up:
      case Direction.Down:
        oscillation = transform.right * deltaS;
        break;
      case Direction.Left:
      case Direction.Right:
        oscillation = transform.up * deltaS;
        break;
    }
    return oscillation;
  }
}
