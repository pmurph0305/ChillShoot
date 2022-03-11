using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DamageDisplayPool : PrefabPool<TextDisplayer>
{
  public static Action<Vector3, float> OnEnemyDamagedAction;

  [SerializeField] Camera _camera;

  protected override void OnAwake()
  {
    base.OnAwake();
    _camera = Camera.main;
    OnEnemyDamagedAction += OnCreateElementHandler;
  }

  public void OnCreateElementHandler(Vector3 position, float damage)
  {
    Vector3 v = _camera.WorldToScreenPoint(position);
    v = position;
    // v.z = 0;
    damage = UnityEngine.Random.Range(0, 99);
    TextDisplayer td = Get(v);
    td.SetText(damage.ToString());
  }
}
