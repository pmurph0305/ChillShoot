using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExperienceAttractor : MonoBehaviour
{

  private void Awake()
  {
    if (AlwaysActive)
    {
      EnableCollider(true);
    }
    else
    {
      EnableCollider(false);
    }
    OnAwake();
  }

  protected virtual void OnAwake() { }


  protected ExperiencePickup ep;

  Timer pickupTimer;
  [SerializeField] bool AlwaysActive;
  public void Activate(float duration)
  {
    if (AlwaysActive) return;
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
