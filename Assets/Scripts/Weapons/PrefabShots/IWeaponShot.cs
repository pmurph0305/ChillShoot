using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponShot
{
  public float GetShotDamage();
  public Transform GetTransform();
  public float GetShotKnockback();
}
