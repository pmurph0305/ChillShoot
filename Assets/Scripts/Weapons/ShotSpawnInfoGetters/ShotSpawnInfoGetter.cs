using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotSpawnInfoGetter : MonoBehaviour, IGetShotSpawnInfo
{
  [SerializeField] protected Vector3 SpawnScale = Vector3.one;

  /// <summary>
  /// Gets the position to spawn a weapon's shot at.
  /// </summary>
  /// <returns></returns>
  protected abstract Vector3 GetSpawnPosition();

  /// <summary>
  /// Gets the rotation to spawn a weapon's shot with
  /// </summary>
  /// <returns></returns>
  protected abstract Quaternion GetSpawnRotation();

  /// <summary>
  /// Gets the position and rotation with which to spawn a weapon's shot.
  /// (So we can manage different things before/after getting a position and rotation, like iterating a list of spawn locations.)
  /// </summary>
  /// <returns></returns>
  public abstract TransformSpawnInfo GetTransformSpawnInfo();

  public virtual Vector3 GetScale() { return SpawnScale; }

}
