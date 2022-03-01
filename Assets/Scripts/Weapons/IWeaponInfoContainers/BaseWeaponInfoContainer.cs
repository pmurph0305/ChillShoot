using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An abstract class so we can create different weapon info providers.<br></br>
/// This way we can create different functionality of weaponinfos more easily.
/// </summary>
public abstract class BaseWeaponInfoContainer : MonoBehaviour, IWeaponInfoContainer
{

  /// <summary>
  /// Gets the WeaponInfo class for this container
  /// </summary>
  /// <returns></returns>
  public abstract WeaponInfo GetWeaponInfo();
}
