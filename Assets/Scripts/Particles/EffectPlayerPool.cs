using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EffectPlayerPool : PrefabPool<EffectPlayer>
{
  public static Action<Vector3> StartEffect;

  protected override void OnAwake()
  {
    base.OnAwake();
    StartEffect += OnStartEffectHandler;
  }

  void OnStartEffectHandler(Vector3 position)
  {
    Get(position);
  }
}
