using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary
{

  public static Dictionary<Transform, EnemyController> activeDict = new Dictionary<Transform, EnemyController>();
  public static Dictionary<Transform, EnemyController> dict = new Dictionary<Transform, EnemyController>();



  public static EnemyController Get(Transform key)
  {
    return dict[key];
  }

  public static EnemyController GetActive(Transform key)
  {
    return activeDict[key];
  }

  public static void Add(Transform key, EnemyController value)
  {
    dict.Add(key, value);
  }

  public static void AddActive(Transform key, EnemyController value)
  {
    activeDict.Add(key, value);
  }

  public static void RemoveActive(Transform key)
  {
    activeDict.Remove(key);
  }

  public static bool Contains(Transform key)
  {
    return dict.ContainsKey(key);
  }

  public static bool ContainsActive(Transform key)
  {
    return activeDict.ContainsKey(key);
  }
}
