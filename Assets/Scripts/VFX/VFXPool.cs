using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VFXPool : PrefabPool<VFXPlayer>
{
  [SerializeField] public bool RotateToDirection;
  [SerializeField] public bool RaycastForPosition;

  // public override VFXPlayer Get(Vector3 position)
  // {
  //   if (position == Vector3.zero)
  //   {
  //     Debug.LogError("zero position get.");
  //   }
  //   return base.Get(position);
  // }

  // public override VFXPlayer Get(Vector3 position, Quaternion rotation)
  // {
  //   if (position == Vector3.zero)
  //   {
  //     Debug.LogError("zero position get.");
  //   }
  //   return base.Get(position, rotation);
  // }
}
