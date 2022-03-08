using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelDirector : MonoBehaviour
{
  [SerializeField] protected Vector3 travelDirection;

  public virtual void OnGetFromPool()
  {
    travelDirection = GetNewTravelDirection();
  }

  protected abstract Vector3 GetNewTravelDirection();

  public virtual Vector3 GetTravelDirection()
  {
    return travelDirection;
  }

  public virtual void UpdateMovement(float movementSpeed, float deltaTime)
  {
    transform.position += GetScaledMovement(movementSpeed, deltaTime);
  }

  /// <summary>
  /// Gets movement vector for this frame.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <returns>Movement vector already scaled by time.delta time.</returns>
  public virtual Vector3 GetScaledMovement(float movementSpeed, float deltaTime)
  {
    return deltaTime * movementSpeed * GetTravelDirection();
  }
}
