using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabShot3D : PrefabShotBase
{
  // [Header("3D")]
  Rigidbody rb;
  // [SerializeField] Collider col;

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();
    if (rb != null && rb.isKinematic)
    {
      rb = null;
    }
  }

  protected override void UpdateMovement()
  {
    if (rb == null)
    {
      director.UpdateMovement(GetSpeed(), Time.deltaTime, false);
    }
  }

  protected override void UpdateFixedMovement()
  {
    if (rb != null)
    {
      director.UpdateMovement(GetSpeed(), Time.fixedDeltaTime, false);
    }
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
