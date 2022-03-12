using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : PrefabPool<EnemyController>
{
  // protected override void OnAwake()
  // {
  //   base.OnAwake();

  // }

  // HashSet<EnemyController> Active = new HashSet<EnemyController>();

  // private void FixedUpdate()
  // {
  //   foreach (var item in Active)
  //   {
  //     item.OnFixedUpdate();
  //   }
  // }

  // protected override void OnGetFromPool(EnemyController item)
  // {
  //   base.OnGetFromPool(item);
  //   Active.Add(item);
  // }

  // protected override void OnReturnedToPool(EnemyController item)
  // {
  //   base.OnReturnedToPool(item);
  //   Active.Remove(item);
  // }
}
