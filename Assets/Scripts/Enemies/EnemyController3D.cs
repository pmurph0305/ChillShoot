using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController3D : EnemyControllerBase
{
  [SerializeField] Rigidbody rb;
  [SerializeField] Collider col;

  protected override void UpdateMovement(float speed, bool useFixedUpdate)
  {
    if (useFixedUpdate)
    {
      travelDirector.UpdateMovement(speed, Time.fixedDeltaTime, false);
    }
    else
    {
      travelDirector.UpdateMovement(speed, Time.deltaTime, false);
    }
  }
}
