using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickerUpper : MonoBehaviour
{
  [SerializeField] PlayerController controller;

  ExperiencePickup ep;
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other, out ep))
    {
      controller.GainExp(ep.ExperienceValue);
      ep.Release();
    }
  }
}
