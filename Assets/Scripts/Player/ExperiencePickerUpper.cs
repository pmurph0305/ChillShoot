using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ExperiencePickerUpper : MonoBehaviour
{
  [SerializeField] PlayerController controller;

  ExperiencePickup ep;

  public static event Action<float> OnPlayerGainExpAction;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other.transform, out ep))
    {
      // Debug.Log("Pickup and release.", ep);
      // controller.GainExp(ep.ExperienceValue);
      // PlayerInfo.OnPlayerGainExpAction(ep.ExperienceValue);
      OnPlayerGainExpAction?.Invoke(ep.ExperienceValue);
      ep.Release();
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (ExperiencePickupPool.TryGetActiveExperiencePickup(other.transform, out ep))
    {
      // Debug.Log("Pickup and release.", ep);
      // controller.GainExp(ep.ExperienceValue);
      // PlayerInfo.OnPlayerGainExpAction(ep.ExperienceValue);
      OnPlayerGainExpAction?.Invoke(ep.ExperienceValue);
      ep.Release();
    }
  }
}
