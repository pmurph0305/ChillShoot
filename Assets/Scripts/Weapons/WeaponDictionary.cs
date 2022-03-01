using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDictionary
{
  static Dictionary<string, WeaponInfo> dict = new Dictionary<string, WeaponInfo>();

  public static WeaponInfo Get(string key)
  {
    return dict[key];
  }

  public static void Add(string key, WeaponInfo value)
  {
    dict.Add(key, value);
  }
}
