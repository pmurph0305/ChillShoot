using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformDirector : TravelDirector
{
  [Header("Player Transform Director")]
  [SerializeField] Direction directionFromPlayer;
  protected override Vector3 GetNewTravelDirection()
  {
    switch (directionFromPlayer)
    {
      case Direction.Up:
        return PlayerController.PlayerTransform.up;
      case Direction.Down:
        return -PlayerController.PlayerTransform.up;
      case Direction.Left:
        return -PlayerController.PlayerTransform.right;
      case Direction.Right:
        return PlayerController.PlayerTransform.right;
      case Direction.Forward:
        return PlayerController.PlayerTransform.forward;
      case Direction.Back:
        return -PlayerController.PlayerTransform.forward;
    }
    return Vector3.zero;
  }
}
