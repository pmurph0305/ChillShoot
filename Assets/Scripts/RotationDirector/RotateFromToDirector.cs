using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFromToDirector : RotationDirector
{

  private void Awake()
  {
    from = Quaternion.Euler(FromRotation);
    to = Quaternion.Euler(ToRotation);
    angle = Quaternion.Angle(from, to);
  }

  [SerializeField] Vector3 FromRotation;
  [SerializeField] Vector3 ToRotation;
  [SerializeField] float rotationTime = 1.0f;
  Quaternion from;
  Quaternion to;
  float angle = 0.0f;
  bool goingTo;
  protected override Quaternion GetNewRotation()
  {
    goingTo = true;
    return from;
  }


  public override Quaternion GetScaledRotation(float rotationSpeed, float deltaTime)
  {
    if (goingTo && transform.rotation == to)
    {
      goingTo = false;
    }
    else if (!goingTo && transform.rotation == from)
    {
      goingTo = true;
    }
    if (goingTo)
    {
      return Quaternion.RotateTowards(transform.rotation, to, deltaTime / rotationTime * angle);
    }
    else
    {
      return Quaternion.RotateTowards(transform.rotation, from, deltaTime / rotationTime * angle);
    }
    // t += deltaTime;
    // return Quaternion.RotateTowards(from, to, )
  }

}
