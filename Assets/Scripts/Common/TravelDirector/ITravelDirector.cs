using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITravelDirector
{
  void ResetTravelDirector();


  /// <summary>
  /// Sets an additional velocity to be included in the travel directors movement.
  /// </summary>
  /// <param name="velocity"></param>
  void SetAdditionalVelocity(Vector3 velocity);


  /// <summary>
  /// Sets an instantaenous velocity vector to be included in the travel directors movement.
  /// This vector is not scaled by deltaTime when calculating movement.
  /// </summary>
  /// <param name="velocity"></param>
  void SetInstantVelocity(Vector3 velocity);

  /// <summary>
  /// Gets the over-all travel direction of the object.
  /// Without considering anything like offsets.
  /// </summary>
  /// <returns></returns>
  Vector3 GetTravelDirection();


  /// <summary>
  /// Updates the position of the transform this script is on.
  /// If a non-kinematic rigidbody or rigidbody2d is attached to the object, the rigidbody is moved using rigidbody.moveposition.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <param name="deltaTime"></param>
  void UpdateMovement(float movementSpeed, float deltaTime, bool is2d);

  /// <summary>
  /// Gets the movement vector for this frame, also rotates the object if there is a rotation director or face travel direction is enabled.
  /// Note that this also increases the offsets timer, so you will need to move the object yourself.
  /// Call UpdateMovement instead if you want this script to move it.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <param name="deltaTime"></param>
  /// <param name="is2d"></param>
  /// <returns></returns>
  Vector3 GetMovementVector(float movementSpeed, float deltaTime, bool is2d);

}
