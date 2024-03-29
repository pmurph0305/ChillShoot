using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base weapon class that handles getting the weaponinfo from the weapon container, handling the cooldown timer of the weapon, and telling the weapon to shoot.
/// WeaponInfo's are added to the static dictionary by the gameobjects tag that this script is on.
/// So each weapon should have a unique tag.
/// </summary>
public abstract class Weapon : MonoBehaviour
{
  [SerializeField] BaseWeaponInfoContainer weaponInfoContainer;
  protected WeaponInfo weaponInfo;
  [SerializeField] protected Timer timer;

  private void Start()
  {
    weaponInfo = weaponInfoContainer.GetWeaponInfo();
    AddWeaponInfoToDictionary();
    timer = new Timer(GetWeaponCooldown());
    OnStart();
  }

  /// <summary>
  /// Adds the weapon info for this weapon to the global weapon info dictionary.
  /// </summary>
  public virtual void AddWeaponInfoToDictionary()
  {
    WeaponDictionary.Add(weaponInfo);
  }

  protected virtual float GetWeaponCooldown()
  {
    return weaponInfo.GetCooldown();
  }

  private void Update()
  {
    UpdateCooldownTimer();
    OnUpdate();
  }

  protected virtual void UpdateCooldownTimer()
  {
    if (timer.EndTime != GetWeaponCooldown())
    {
      // timer = new Timer(weaponInfo.Cooldown);
      timer = new Timer(GetWeaponCooldown(), timer.CurrentTime);
    }
    if (timer.Update())
    {
      Shoot();
      timer.Reset(true);
    }
  }

  /// <summary>
  /// Called at the end of update.
  /// </summary>
  protected virtual void OnUpdate()
  {

  }

  /// <summary>
  /// Called at the end of start.
  /// </summary>
  protected virtual void OnStart()
  {

  }

  /// <summary>
  /// Called when the weapon should shoot
  /// </summary>
  protected abstract void Shoot();
}
