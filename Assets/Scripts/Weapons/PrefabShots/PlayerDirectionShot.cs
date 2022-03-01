using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionShot : PrefabShot
{
  public override void OnGetFromPool()
  {
    base.OnGetFromPool();
    travelDirection = PlayerController.PlayerTransform.up;
  }
}
