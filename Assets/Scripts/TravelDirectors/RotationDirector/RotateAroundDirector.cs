using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundDirector : RotationDirector
{
  [Header("RotateAroundDirector")]
  [SerializeField] Vector3 axis = Vector3.forward;
  public override void UpdateTransform(float deltaTime)
  {
    transform.RotateAround(transform.position, axis, rotationSpeed * deltaTime);
  }

}