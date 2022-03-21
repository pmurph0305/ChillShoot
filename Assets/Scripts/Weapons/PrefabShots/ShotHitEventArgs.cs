using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShotHitEventArgs
{
  public WeaponKey key;
  public Vector3 ShotPosition;
  public Vector3 EnemyPosition;
  public Vector3 ShotTravelDirection;

  public ShotHitEventArgs(WeaponKey key, Vector3 shotPosition, Vector3 enemyPosition, Vector3 travelDir)
  {
    this.key = key;
    this.ShotPosition = shotPosition;
    this.EnemyPosition = enemyPosition;
    this.ShotTravelDirection = travelDir;
  }
}
