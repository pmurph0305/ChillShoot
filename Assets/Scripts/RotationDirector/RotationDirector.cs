using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDirector : MonoBehaviour
{
  [SerializeField] protected float rotationSpeed;
  protected Quaternion rotation;
  public virtual void OnGetFromPool()
  {
    rotation = GetNewRotation();
  }

  protected virtual Quaternion GetNewRotation()
  {
    return Quaternion.identity;
  }

  public virtual void UpdateTransform(float deltaTime)
  {
    transform.rotation = GetScaledRotation(rotationSpeed, deltaTime);
  }


  public virtual Quaternion GetScaledRotation(float rotationSpeed, float deltaTime)
  {
    return rotation;
  }
}
