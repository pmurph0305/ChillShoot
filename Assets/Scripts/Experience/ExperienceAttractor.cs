using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExperienceAttractor : MonoBehaviour
{

  protected ExperiencePickup ep;

  Timer pickupTimer;
  public void Activate(float duration)
  {
    pickupTimer = new Timer(duration);
    EnableCollider(true);
  }

  protected abstract void EnableCollider(bool active);

  private void Update()
  {
    if (pickupTimer != null)
    {
      if (pickupTimer.Update())
      {
        EnableCollider(false);
        pickupTimer = null;
      }
    }
  }

}
