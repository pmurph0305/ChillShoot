using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelDirector : MonoBehaviour
{
  [Header("Travel Director")]
  [SerializeField] protected TravelOffsetter offsetter;
  [SerializeField] RotationDirector rotationDirector;
  [SerializeField] protected bool FaceTravelDirection = true;
  [SerializeField] protected Transform visual;


  [SerializeField] protected Vector3 travelDirection;
  [SerializeField] protected Vector3 additionalVelocity;
  [SerializeField] protected Vector3 instantVelocity;

  public virtual void OnGetFromPool()
  {
    travelDirection = GetNewTravelDirection();
    if (offsetter != null)
    {
      offsetter.ResetOffset();
    }
    if (rotationDirector != null)
    {
      rotationDirector.OnGetFromPool();
    }
    if (FaceTravelDirection && visual == null)
    {
      // Debug.LogWarning("No visual transform set, so face direction will not work correctly, but setting to this.transform.");
      visual = this.transform;
    }
  }


  public void SetAdditionalVelocity(Vector3 velocity)
  {
    additionalVelocity = velocity;
  }

  public void SetInstantVelocity(Vector3 velocity)
  {
    instantVelocity = velocity;
  }

  public virtual Vector3 GetInitialPosition()
  {
    if (offsetter == null) return transform.position;
    return transform.position + offsetter.GetOffset(0);
  }

  /// <summary>
  /// Called when the travel director is gotten from the pool, used to calculate a new travelDirection
  /// </summary>
  /// <returns></returns>
  protected abstract Vector3 GetNewTravelDirection();

  /// <summary>
  /// Gets the travel direction, can be overridden to update the travel direction each time it is called.
  /// </summary>
  /// <returns></returns>
  public virtual Vector3 GetTravelDirection()
  {
    return travelDirection;
  }

  /// <summary>
  /// Updates the position of the transform this script is on.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <param name="deltaTime"></param>
  public virtual void UpdateMovement(float movementSpeed, float deltaTime)
  {
    transform.position += GetScaledMovement(movementSpeed, deltaTime);
    UpdateRotation(deltaTime);
  }

  public virtual void UpdateMovement(Rigidbody2D rb2d, float movementSpeed, float deltaTime)
  {
    rb2d.MovePosition(rb2d.position + (Vector2)GetScaledMovement(movementSpeed, deltaTime));
    UpdateRotation(deltaTime);
  }

  public virtual void UpdateMovement(Rigidbody rb, float movementSpeed, float deltaTime)
  {
    rb.MovePosition(rb.position + GetScaledMovement(movementSpeed, deltaTime));
    UpdateRotation(deltaTime);
  }

  protected virtual void UpdateRotation(float deltaTime)
  {
    if (rotationDirector != null)
    {
      rotationDirector.UpdateTransform(deltaTime);
    }
  }



  Vector3 zero = Vector3.zero;

  /// <summary>
  /// Gets the offset from the offsetter if it is used.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  protected virtual Vector3 GetOffset(float deltaTime)
  {
    return offsetter ? offsetter.GetOffset(deltaTime) : zero;
  }

  /// <summary>
  /// Gets movement vector for this frame.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <returns>Movement vector already scaled by time and movementspeed.</returns>
  protected virtual Vector3 GetScaledMovement(float movementSpeed, float deltaTime)
  {
    Vector3 val = deltaTime * movementSpeed * GetTravelDirection() + GetOffset(deltaTime);
    if (FaceTravelDirection && visual != null && deltaTime > 0)
    {
      visual.rotation = Quaternion.LookRotation(Vector3.forward, val);
    }
    Vector3 instantVel = instantVelocity;
    instantVelocity = Vector3.zero;
    return val + additionalVelocity * deltaTime + instantVel;
  }
}
