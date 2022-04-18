using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceAttractor3D : ExperienceAttractor
{
  Collider col;

  private void OnTriggerEnter(Collider other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other.transform, out ep))
    {
      ep.SetTarget(this.transform);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other.transform, out ep))
    {
      ep.SetTarget(null);
    }
  }

  protected override void EnableCollider(bool active)
  {
    col.enabled = active;
  }
}
