using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundDirector : RotationDirector
{
  [SerializeField] Vector3 axis = Vector3.forward;
  public override void UpdateRotation(float rotationSpeed)
  {
    transform.RotateAround(transform.position, axis, rotationSpeed * Time.deltaTime);
  }
}
