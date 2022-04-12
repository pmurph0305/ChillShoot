using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Random = UnityEngine.Random;
public class TestVectors : MonoBehaviour
{
  public bool runTests;
  public int tests = 100000;
  // Start is called before the first frame update
  void Start()
  {

  }

  List<Vector3> vs = new List<Vector3>();

  // Update is called once per frame
  void Update()
  {
    if (runTests)
    {
      for (int i = 0; i < tests; i++)
      {
        vs.Add(new Vector3(Random.Range(-123f, 123f), Random.Range(-123f, 123f), Random.Range(-123f, 123f)));
      }
      Vector3 val = Vector3.zero;
      runTests = false;
      Stopwatch w = new Stopwatch();
      w.Start();

      for (int i = 0; i < tests; i++)
      {
        val = vs[i].normalized;
      }

      TimeSpan t1 = w.Elapsed;

      w.Reset();
      w.Stop();
      w.Start();
      for (int i = 0; i < tests; i++)
      {
        val = vs[i].FastNormalized();
      }

      TimeSpan t2 = w.Elapsed;

      w.Reset();
      w.Stop();
      UnityEngine.Debug.Log("normalized:" + t1.TotalSeconds + " fast:" + t2.TotalSeconds);
    }
  }
}
