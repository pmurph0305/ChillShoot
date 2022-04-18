using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabShot3D : PrefabShotBase
{
  [Header("3D")]
  Rigidbody rb;
  Collider col;

  protected override void UpdateMovement()
  {
    director.UpdateMovement(GetSpeed(), Time.deltaTime, false);
  }

  protected override void UpdateRigidbodyMovement()
  {
    director.UpdateMovement(rb, GetSpeed(), Time.fixedDeltaTime);
  }

  private void OnTriggerEnter(Collider other)
  {
    OnEnterTransform(other.transform);
  }

  private void OnTriggerExit(Collider other)
  {
    OnExitTransform(other.transform);
  }

}
