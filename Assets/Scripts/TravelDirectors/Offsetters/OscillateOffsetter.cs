using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateOffsetter : TravelOffsetter
{

  [SerializeField] Direction transformDirection;
  [SerializeField] float frequency = 10f;
  [SerializeField] float amplitude = 1f;

  float time = 0.0f;
  Vector3 oscillation = Vector3.zero;
  float previousS;

  public override Vector3 GetOffset(float deltaTime)
  {
    time += deltaTime * timeScale;
    float s = amplitude * Mathf.Sin(time * frequency);
    // because otherwise, we're continually adding s, and s increases, we'd be doing it additively
    // by subtracting the previous s, we're using the actual change in S this frame.
    // which is the oscilation we want to add.
    float deltaS = s - previousS;
    previousS = s;
    oscillation = Vector3.zero;
    switch (transformDirection)
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

  protected override void OnReset()
  {
    time = 0.0f;
    previousS = 0.0f;
  }
}
