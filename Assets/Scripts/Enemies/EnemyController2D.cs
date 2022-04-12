using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using System;
public class EnemyController2D : EnemyControllerBase
{

  [SerializeField] Rigidbody2D rb2d;
  [SerializeField] Collider2D col;

  protected override void UpdateMovement(float speed, bool useFixedUpdate)
  {
    if (useFixedUpdate)
    {
      travelDirector.SetAdditionalVelocity(HitKnockback);
      HitKnockback -= HitKnockback * Time.fixedDeltaTime;
      HitKnockback = Vector3.zero;
      travelDirector.UpdateMovement(rb2d, speed, Time.fixedDeltaTime);
      rb2d.AddForce((Vector2)HitKnockback, ForceMode2D.Impulse);
    }
    else
    {
      travelDirector.UpdateMovement(speed, Time.deltaTime);
    }
  }
}

