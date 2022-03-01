using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
  /// <summary>
  /// Get a random position within the bounds.
  /// </summary>
  /// <param name="b"></param>
  /// <returns>Random position within the bounds</returns>
  public static Vector3 RandomWithin(this Bounds b)
  {
    // Vector3 min = b.min;
    // Vector3 max = b.max;
    return new Vector3(Random.Range(b.min.x, b.max.x), Random.Range(b.min.y, b.max.y), Random.Range(b.min.z, b.max.z));
  }
}
