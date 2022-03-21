using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;
using System;
public class VFXPlayer : MonoBehaviour, IPoolable<VFXPlayer>
{
  [SerializeField] VisualEffect vfxGraph;
  IObjectPool<VFXPlayer> pool;

  public void OnCreate()
  {
  }

  public void OnGetFromPool()
  {
    vfxGraph.Play();
  }

  public void Release()
  {
    vfxGraph.Stop();
    this.gameObject.SetActive(false);
  }

  public void SetPool(IObjectPool<VFXPlayer> pool)
  {
    this.pool = pool;
  }

  [SerializeField] int aliveCount;
  [SerializeField] bool anyWake;
  private void Update()
  {
    aliveCount = vfxGraph.aliveParticleCount;
    anyWake = vfxGraph.HasAnySystemAwake();
    if (!anyWake)
    {
      Release();
    }
  }

  private void OnDisable()
  {
    pool.Release(this);
  }
}
