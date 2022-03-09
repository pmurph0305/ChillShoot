using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelDirector : MonoBehaviour
{
  [SerializeField] protected Vector3 travelDirection;
  [SerializeField] protected TravelOffsetter offsetter;
  public virtual void OnGetFromPool()
  {
    travelDirection = GetNewTravelDirection();
    if (offsetter != null)
    {
      offsetter.Reset();
    }
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
  public virtual Vector3 GetScaledMovement(float movementSpeed, float deltaTime)
  {
    return deltaTime * movementSpeed * GetTravelDirection() + GetOffset(deltaTime);
  }
}
