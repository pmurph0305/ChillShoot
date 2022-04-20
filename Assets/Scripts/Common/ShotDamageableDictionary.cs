using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDamageableDictionary
{

  public static Dictionary<Transform, IShotDamageable> activeDict = new Dictionary<Transform, IShotDamageable>();
  public static Dictionary<Transform, IShotDamageable> dict = new Dictionary<Transform, IShotDamageable>();

  public static IShotDamageable Get(Transform key)
  {
    return dict[key];
  }

  public static IShotDamageable GetActive(Transform key)
  {
    return activeDict[key];
  }

  public static void Add(Transform key, IShotDamageable value)
  {
    dict.Add(key, value);
  }

  public static void AddActive(Transform key, IShotDamageable value)
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
