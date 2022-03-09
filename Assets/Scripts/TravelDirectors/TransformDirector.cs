using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformDirector : TravelDirector
{
  [SerializeField, Tooltip("Oriented according to this transform's axis'")] Transform _transform;
  [SerializeField, Tooltip("Axis direction of specified transform to follow")] protected Direction direction;
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
}
