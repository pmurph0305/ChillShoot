using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectPlayer : MonoBehaviour, IPoolable<EffectPlayer>
{
  [SerializeField] ScaleTweener tweener;
  public void OnCreate()
  {
  }

  public void OnGetFromPool()
  {
    PlayEffect();
  }

  protected void PlayEffect()
  {
    tweener.StartTween(this.transform, Release);
  }

  public void Release()
  {
    this.gameObject.SetActive(false);
  }

  private void OnDisable()
  {
    _pool.Release(this);
  }

  IObjectPool<EffectPlayer> _pool;
  public void SetPool(IObjectPool<EffectPlayer> pool)
  {
    _pool = pool;
  }
}
