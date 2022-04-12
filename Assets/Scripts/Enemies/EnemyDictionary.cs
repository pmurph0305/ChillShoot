using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary
{

  public static Dictionary<Transform, EnemyControllerBase> activeDict = new Dictionary<Transform, EnemyControllerBase>();
  public static Dictionary<Transform, EnemyControllerBase> dict = new Dictionary<Transform, EnemyControllerBase>();



  public static EnemyControllerBase Get(Transform key)
  {
    return dict[key];
  }

  public static EnemyControllerBase GetActive(Transform key)
  {
    return activeDict[key];
  }

  public static void Add(Transform key, EnemyControllerBase value)
  {
    dict.Add(key, value);
  }

  public static void AddActive(Transform key, EnemyControllerBase value)
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
