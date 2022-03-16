using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
  public static float CooldownReduction = 0.0f;

  [SerializeField, Range(0, 1)] float cooldownReduction = 0.0f;

  private void Update()
  {
    if (cooldownReduction != CooldownReduction)
    {
      CooldownReduction = cooldownReduction;
    }
  }
}
