using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDirectionShot : PrefabShot
{
  public override void OnGetFromPool()
  {
    base.OnGetFromPool();
    travelDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
  }
}
