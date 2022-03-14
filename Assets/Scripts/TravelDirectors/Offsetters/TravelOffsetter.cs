using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelOffsetter : MonoBehaviour
{
  [Header("Travel Offsetter")]
  /// <summary>
  /// Amount to scale the offset timer by
  /// </summary>
  [SerializeField, Tooltip("Controls the speed of the offset.")] protected float timeScale = 1.0f;

  // time of the offset
  protected float time;

  // primary world space vector axis.
  protected Vector3 primaryAxis;

  // Vector is calculated once on reset so that you can keep transforming and rotating it without messing things up.
  [SerializeField, Tooltip("Transform's axis that will be used to offset the object.")] protected Direction transformDirection = Direction.Right;

  protected virtual Vector3 CombineWithDirection(float delta)
  {
    return primaryAxis * delta;
  }

  protected Vector3 GetTransformDirection(Direction dir)
  {
    switch (dir)
    {
      case Direction.Up:
        return transform.up;
      case Direction.Down:
        return -transform.up;
      case Direction.Left:
        return -transform.right;
      case Direction.Right:
      default:
        return transform.right;
    }
  }

  /// <summary>
  /// Called when the transform should re-start from zero offset. IE: when the object is gotten from a pool.
  /// </summary>
  public virtual void Reset()
  {
    time = 0.0f;
    primaryAxis = GetTransformDirection(transformDirection);
    OnReset();
  }

  /// <summary>
  /// Reset the variables used to calculate offsets
  /// </summary>
  protected abstract void OnReset();


  /// <summary>
  /// Gets the offset after increasing the offset calculations time by deltaTime.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  public virtual Vector3 GetOffset(float deltaTime)
  {
    time += deltaTime * timeScale;
    return CalculateOffset();
  }

  /// <summary>
  /// Gets the total additional movement for this frame.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  protected abstract Vector3 CalculateOffset();




}
