using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentDelayShot : PrefabShot
{
  [SerializeField] float detachTime = 0.1f;
  Timer parentDelayTimer;

  public override void OnCreate()
  {
    base.OnCreate();
    parentDelayTimer = new Timer(detachTime);
  }

  public override void OnGetFromPool()
  {
    base.OnGetFromPool();
    parentDelayTimer.Reset(true);
  }

  protected override void OnUpdate(float deltaTime)
  {
    base.OnUpdate(deltaTime);
    if (parentDelayTimer.Update(deltaTime))
    {
      this.transform.SetParent(null);
    }
  }
}
