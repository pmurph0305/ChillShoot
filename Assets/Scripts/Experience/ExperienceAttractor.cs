using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceAttractor : MonoBehaviour
{
  [SerializeField] Collider2D col;
  ExperiencePickup ep;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other, out ep))
    {
      ep.SetTarget(this.transform);
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other, out ep))
    {
      ep.SetTarget(null);
    }
  }

  Timer pickupTimer;
  public void Activate(float duration)
  {
    pickupTimer = new Timer(duration);
    col.enabled = true;
  }

  private void Update()
  {
    if (pickupTimer != null)
    {
      if (pickupTimer.Update())
      {
        col.enabled = false;
        pickupTimer = null;
      }
    }
  }

}
