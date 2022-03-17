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
      //todo: area causes this to not work correctly.
      // box collider is way too large when scaling non-uniformly.
      // but with the collider settings now its... good enough
      Vector3 dir = target.position - transform.position;
      Quaternion rotation = Quaternion.LookRotation(Vector3.forward, dir);
      transformToStretch.rotation = rotation;
      float distance = Vector3.Distance(target.position, transform.position);
      targetMatcher.position = target.position;
      transformToStretch.position = transform.position + (dir.normalized) * distance / 2;
      Vector3 s = transform.localScale;
      s.y = distance / (BaseLength * transform.lossyScale.y);
      transformToStretch.localScale = s;
    }
  }
  private void Update()
  {
    TryStretchToTarget();
  }

}
