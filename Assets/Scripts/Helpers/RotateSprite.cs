using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{


  [SerializeField] float RotateSpeed = 180f;

  // Update is called once per frame
  void Update()
  {
    transform.RotateAround(transform.position, Vector3.forward, RotateSpeed * Time.deltaTime);
  }
}
