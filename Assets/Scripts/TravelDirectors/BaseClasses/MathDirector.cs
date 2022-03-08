using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MathDirector : TravelDirector
{
  [SerializeField] Transform _transform;

  [SerializeField] protected Direction direction;
  public enum Direction
  {
    Up,
    Down,
    Left,
    Right
  }

  protected override Vector3 GetNewTravelDirection()
  {
    switch (direction)
    {
      case Direction.Up:
        return _transform.up;
      case Direction.Down:
        return -_transform.up;
      case Direction.Left:
        return -_transform.right;
      default:
        return _transform.right;
    }
  }

  protected abstract Vector3 GetMathVector();

  public override Vector3 GetScaledMovement(float movementSpeed, float deltaTime)
  {
    return GetTravelDirection() * deltaTime * movementSpeed + GetMathVector();
  }
}
