using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionFacer : MonoBehaviour
{
  public void SetDirection(Vector3 direction)
  {
    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
  }
}
