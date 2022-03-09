using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanInit : MonoBehaviour
{
  [SerializeField] int maxSimultaneousTweens = 2048;
  // Start is called before the first frame update
  void Awake()
  {
    LeanTween.init(maxSimultaneousTweens);
  }
}
