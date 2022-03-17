using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchToTarget : MonoBehaviour
{
  [SerializeField] PrefabShot shot;
  [SerializeField] protected ITargetProvider targetProvider;
  [SerializeField] Transform targetMatcher;
  [SerializeField] Transform transformToStretch;
  TargetedWeaponInfo twi;

  [SerializeField] float BaseLength = 1.0f;
  private void Awake()
  {
    shot.OnGetFromPoolAction += OnGetFromPool;
    targetProvider = GetComponent<ITargetProvider>();
  }

  [SerializeField] Transform target;
  void OnGetFromPool()
  {
    TryStretchToTarget();
  }

  void TryStretchToTarget()
  {
    target = targetProvider.GetTarget();
    if (target != null)
    {
      Vector3 dir = target.position - transform.position;
      Quaternion rotation = Quaternion.LookRotation(Vector3.forward, dir);
      transformToStretch.rotation = rotation;
      float distance = Vector3.Distance(target.position, transform.position);
      transformToStretch.position = transform.position + (dir.normalized) * distance / 2;
      Vector3 s = transform.localScale;
      s.y = distance / BaseLength;
      transformToStretch.localScale = s;
    }
  }
  private void Update()
  {
    TryStretchToTarget();
  }

}
