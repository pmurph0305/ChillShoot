using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiAxisOffsetter : TravelOffsetter
{
  [Header("MultiAxis Offsetter")]
  [SerializeField] protected Direction secondaryTransformDirection = Direction.Up;
  protected Vector3 secondaryAxis;
  protected override void OnReset()
  {
    secondaryAxis = GetTransformDirection(secondaryTransformDirection);
  }

}
