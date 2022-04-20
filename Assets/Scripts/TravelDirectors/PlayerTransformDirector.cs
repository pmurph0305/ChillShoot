using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformDirector : TravelDirector
{


  protected override void OnAwake()
  {
    base.OnAwake();
    if (_PlayerTransform == null && PlayerTransform != null)
    {
      _PlayerTransform = PlayerTransform;
    }
  }

  public static Transform _PlayerTransform;
  [Header("Player Transform Director")]
  public Transform PlayerTransform;
  [SerializeField] Direction directionFromPlayer;
  protected override Vector3 GetNewTravelDirection()
  {
    if (_PlayerTransform == null) return Vector3.zero;
    switch (directionFromPlayer)
    {
      case Direction.Up:
        return _PlayerTransform.up;
      case Direction.Down:
        return -_PlayerTransform.up;
      case Direction.Left:
        return -_PlayerTransform.right;
      case Direction.Right:
        return _PlayerTransform.right;
      case Direction.Forward:
        return _PlayerTransform.forward;
      case Direction.Back:
        return -_PlayerTransform.forward;
    }
    return Vector3.zero;
  }
}
