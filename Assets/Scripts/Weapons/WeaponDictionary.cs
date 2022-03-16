using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDictionary
{
  static Dictionary<WeaponKey, WeaponInfo> dict = new Dictionary<WeaponKey, WeaponInfo>();

  public static WeaponInfo Get(WeaponKey key)
  {
    return dict[key];
  }

  // public static void Add(string key, WeaponInfo value)
  // {
  //   dict.Add(key, value);
  // }

  public static void Add(WeaponInfo value)
  {
    dict.Add(value.key, value);
  }
}
