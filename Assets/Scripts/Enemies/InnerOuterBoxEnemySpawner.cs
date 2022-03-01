using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerOuterBoxEnemySpawner : EnemySpawner
{
  Camera c;
  Bounds innerBounds;
  Bounds outerBounds;

  [SerializeField] BoxCollider2D innerBox;
  [SerializeField] BoxCollider2D outerBox;
  protected override void OnStart()
  {
    base.OnStart();
    c = Camera.main;
    innerBounds = innerBox.bounds;
    outerBounds = outerBox.bounds;
  }

  Vector3 GetPositionOffscreen()
  {
    Vector3 pos = Vector3.zero;
    int i = 0;
    while (innerBounds.Contains(pos) && i < 1000)
    {
      i++;
      pos = outerBounds.RandomWithin();
    }
    return pos + PlayerController.PlayerPosition;
  }

  protected override Vector3 GetSpawnPosition()
  {
    return GetPositionOffscreen();
  }
}
