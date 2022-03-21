using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VFXPool : PrefabPool<VFXPlayer>
{
  [SerializeField] public bool RotateToDirection;
  [SerializeField] public bool RaycastForPosition;
}
