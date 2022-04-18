using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;


//TODO: A separate component that just returns the travel direction, so we can use different combinations and mix and match.

/// <summary>
/// An abstract class so we can create different types of Prefab Shots and place those onto items but retain the same pool and dictionary functionality.
/// Responsible for tracking its own life-time, updating its movement, and getting its travel direction for updating movement.
/// Also responsible for disabling/releasing to the pool when it hits an enemy (OnHitEnemy is called from the WeaponInfo class)
/// </summary>
public class PrefabShot2D : PrefabShotBase
{
  [Header("2D")]
  [SerializeField] Rigidbody2D rb;
  [SerializeField] Collider2D col;

  protected override void UpdateRigidbodyMovement()
  {
    director.UpdateMovement(rb, GetSpeed(), Time.fixedDeltaTime);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    OnEnterTransform(other.transform);
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    OnExitTransform(other.transform);
  }

}