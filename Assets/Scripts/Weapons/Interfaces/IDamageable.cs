using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IDamageable
{
  public bool isActiveAndEnabled();

  public Transform GetTransform();

  public event Action<IDamageable> OnEnemyReleased;

  public void OnHitFromShot(IWeaponShot shot);

  public void OnHitForDamage(float damage);
}
