using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationDirector : MonoBehaviour
{

  [SerializeField] protected Quaternion rotation;
  public virtual void OnGetFromPool()
  {
    rotation = GetNewRotation();
  }

  protected virtual Quaternion GetNewRotation()
  {
    return Quaternion.identity;
  }

  public virtual void UpdateRotation(float rotationSpeed)
  {
    transform.rotation = GetScaledRotation(rotationSpeed);
  }

  public virtual Quaternion GetScaledRotation(float rotationSpeed)
  {
    return rotation;
  }
}
