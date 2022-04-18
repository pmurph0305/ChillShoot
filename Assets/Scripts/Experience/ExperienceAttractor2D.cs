using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceAttractor2D : ExperienceAttractor
{
  [SerializeField] Collider2D col;
  // Start is called before the first frame update
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other.transform, out ep))
    {
      ep.SetTarget(this.transform);
    }
  }

  private void OnTriggerExit2D(Collider2D other)
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
