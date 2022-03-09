using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelOffsetter : MonoBehaviour
{
  /// <summary>
  /// Amount to scale the offset timer by
  /// </summary>
  [SerializeField, Tooltip("Controls the speed of the offset.")] protected float timeScale = 1.0f;

  /// <summary>
  /// Called when the transform should re-start from zero offset. IE: when the object is gotten from a pool.
  /// </summary>
  public virtual void Reset()
  {
    OnReset();
  }

  /// <summary>
  /// Reset the variables used to calculate offsets
  /// </summary>
  protected abstract void OnReset();

  /// <summary>
  /// Gets the total additional movement for this frame.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  public abstract Vector3 GetOffset(float deltaTime);

}
