using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DirectionOffsetter : TravelOffsetter
{
  [SerializeField] protected Direction transformDirection = Direction.Right;

  protected virtual Vector3 CombineWithDirection(float delta)
  {
    switch (transformDirection)
    {
      case Direction.Up:
        return transform.up * delta;
      case Direction.Down:
        return -transform.up * delta;
      case Direction.Left:
        return -transform.right * delta;
      case Direction.Right:
      default:
        return transform.right * delta;
    }
  }
}
