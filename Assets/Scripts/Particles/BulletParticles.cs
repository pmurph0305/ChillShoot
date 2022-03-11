using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticles : MonoBehaviour
{
  [SerializeField] ParticleSystem sys;
  private void OnEnable()
  {
    sys.Play();
  }

  private void OnDisable()
  {
    sys.Stop();
  }
}
