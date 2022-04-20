using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EffectPlayerPool : PrefabPool<EffectPlayer>
{
  protected override void OnAwake()
  {
    base.OnAwake();
    PrefabShotBase.OnHitEnemyAction += OnHitEnemyActionHandler;
  }

  private void OnHitEnemyActionHandler(ShotHitEventArgs obj)
  {
    OnStartEffectHandler(obj.EnemyPosition);
  }

  void OnStartEffectHandler(Vector3 position)
  {
    Get(position);
  }
}
