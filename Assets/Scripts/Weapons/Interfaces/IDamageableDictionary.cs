using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDamageableDictionary
{

  public static Dictionary<Transform, IDamageable> activeDict = new Dictionary<Transform, IDamageable>();
  public static Dictionary<Transform, IDamageable> dict = new Dictionary<Transform, IDamageable>();

  public static IDamageable Get(Transform key)
  {
    return dict[key];
  }

  public static IDamageable GetActive(Transform key)
  {
    return activeDict[key];
  }

  public static void Add(Transform key, IDamageable value)
  {
    dict.Add(key, value);
  }

  public static void AddActive(Transform key, IDamageable value)
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
