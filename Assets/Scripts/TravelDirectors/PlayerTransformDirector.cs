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
        return PlayerInfo.PlayerTransform.up;
      case Direction.Down:
        return -PlayerInfo.PlayerTransform.up;
      case Direction.Left:
        return -PlayerInfo.PlayerTransform.right;
      case Direction.Right:
        return PlayerInfo.PlayerTransform.right;
      case Direction.Forward:
        return PlayerInfo.PlayerTransform.forward;
      case Direction.Back:
        return -PlayerInfo.PlayerTransform.forward;
    }
    return Vector3.zero;
  }
}
