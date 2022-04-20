using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IShotDamageable
{
  public bool isActiveAndEnabled();

  public Transform GetTransform();

  public event Action<IShotDamageable> OnEnemyReleased;

  public void OnHitFromShot(IWeaponShot shot);

  void OnHitFromAbility(float damage);
}
