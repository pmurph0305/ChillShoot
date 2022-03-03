using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary
{

  public static Dictionary<Collider2D, EnemyController> activeDict = new Dictionary<Collider2D, EnemyController>();
  public static Dictionary<Collider2D, EnemyController> dict = new Dictionary<Collider2D, EnemyController>();

  public static EnemyController Get(Collider2D key)
  {
    return dict[key];
  }

  public static EnemyController GetActive(Collider2D key)
  {
    return activeDict[key];
  }

  public static void Add(Collider2D key, EnemyController value)
  {
    dict.Add(key, value);
  }

  public static void AddActive(Collider2D key, EnemyController value)
  {
    activeDict.Add(key, value);
  }

  public static void RemoveActive(Collider2D key)
  {
    activeDict.Remove(key);
  }

  public static bool Contains(Collider2D key)
  {
    return dict.ContainsKey(key);
  }

  public static bool ContainsActive(Collider2D key)
  {
    return activeDict.ContainsKey(key);
  }
}
