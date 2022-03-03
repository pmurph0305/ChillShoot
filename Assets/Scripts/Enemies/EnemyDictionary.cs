using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary
{
  public static Dictionary<Collider2D, EnemyController> dict = new Dictionary<Collider2D, EnemyController>();

  public static EnemyController Get(Collider2D key)
  {
    return dict[key];
  }

  public static void Add(Collider2D key, EnemyController value)
  {
    dict.Add(key, value);
  }

  public static void Remove(Collider2D key)
  {
    dict.Remove(key);
  }

  public static bool Contains(Collider2D key)
  {
    return dict.ContainsKey(key);
  }
}
