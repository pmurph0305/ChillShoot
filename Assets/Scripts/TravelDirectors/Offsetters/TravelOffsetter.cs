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
  [SerializeField, Tooltip("When true, the offset is always relative to the transforms rotation. Otherwise, the direction is calculated once on spawn.")] bool AlwaysRelativeToTransform;
  protected virtual Vector3 CombineWithDirection(float delta, Vector3 dir)
  {
    return dir * delta;
  }
  protected virtual Vector3 CombineWithDirection(float delta)
  {
    return (AlwaysRelativeToTransform ? GetTransformDirection(transformDirection) : primaryAxis) * delta;
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
        return transform.right;
      case Direction.Forward:
        return transform.forward;
      case Direction.Back:
        return -transform.forward;
      default:
        return transform.right;
    }
  }

  /// <summary>
  /// Called when the transform should re-start from zero offset. IE: when the object is gotten from a pool.
  /// </summary>
  public virtual void ResetOffset()
  {
    time = 0.0f;
    primaryAxis = GetTransformDirection(transformDirection);
    OnResetOffset();
    // sets up the "previous" value that offsets use, so the object doesn't jump position.
    CalculateOffset();
  }

  /// <summary>
  /// Reset the variables used to calculate offsets
  /// </summary>
  protected abstract void OnResetOffset();



  /// <summary>
  /// Gets the offset after increasing the offset calculations time by deltaTime.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  public virtual Vector3 GetOffset(float deltaTime)
  {
    AddTime(deltaTime);
    return CalculateOffset();
  }

  protected virtual void AddTime(float deltaTime)
  {
    time += deltaTime * timeScale;
  }

  /// <summary>
  /// Gets the total additional movement for this frame.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  protected abstract Vector3 CalculateOffset();




}
