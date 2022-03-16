using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponInfo
{
  [SerializeField] protected WeaponKey _key;
  public WeaponKey key { get { return _key; } private set { _key = value; } }


  public float ShotDamage = 10f;
  public float ShotSpeed = 5f;
  [SerializeField] protected float ShotLifeTime = 2f;
  public float GetLifeTime()
  {
    return ShotLifeTime * PlayerData.DurationMultiplier;
  }

  [SerializeField] protected float Cooldown = 0.5f;

  public float GetCooldown()
  {
    return Cooldown * PlayerData.CooldownMultiplier;
  }

  [SerializeField] protected float ScaleMultiplier = 1.0f;
  public float GetScaleMultiplier()
  {
    return ScaleMultiplier * PlayerData.AreaMultiplier;
  }


  public float DamageCooldown = 0.25f;
  public bool DestroyOnHit = true;
  public int DestroyAfterXHits = 1;
  public bool AddPlayerSpeed = true;

  /// <summary>
  /// Dictionary used by enemies to tell the weapon info that a WeaponShot has hit an enemy.<br></br> 
  /// The WeaponInfoClass then tells the shot it hit something, and the shot itself handles returning itself to the proper pool.
  /// </summary>
  /// <typeparam name="Collider2D"></typeparam>
  /// <typeparam name="IWeaponShot"></typeparam>
  /// <returns></returns>
  Dictionary<Collider2D, IWeaponShot> ShotDict = new Dictionary<Collider2D, IWeaponShot>();


  /// <summary>
  /// Called when pooled IWeaponShots are created.
  /// </summary>
  /// <param name="shot"></param>
  /// <param name="collider"></param>
  public void Add(IWeaponShot shot, Collider2D collider)
  {
    ShotDict.Add(collider, shot);
  }

  /// <summary>
  /// Called when an enemy is hit by a collider2d that is also a IWeaponShit.
  /// </summary>
  /// <param name="Col"></param>
  public void OnHitEnemy(Collider2D Col, EnemyController enemy)
  {
    IWeaponShot s = null;
    if (ShotDict.TryGetValue(Col, out s))
    {
      s.OnHitEnemy(enemy);
    }
  }

  public void ApplyUpgrade(WeaponUpgradeType type, float value)
  {
    switch (type)
    {
      case WeaponUpgradeType.Damage:
        ApplyDamageUpgrade(value);
        break;
      case WeaponUpgradeType.Area:
        ApplyAreaUpgrade(value);
        break;
      case WeaponUpgradeType.Cooldown:
        ApplyCooldownUpgrade(value);
        break;
      case WeaponUpgradeType.Speed:
        ApplySpeedUpgrade(value);
        break;
    }
  }

  void ApplyDamageUpgrade(float value)
  {
    ShotDamage += ShotDamage * value;
  }

  void ApplyAreaUpgrade(float value)
  {
    ScaleMultiplier += ScaleMultiplier * value;
  }

  void ApplyCooldownUpgrade(float value)
  {
    Cooldown -= Cooldown * value;
    if (Cooldown < 0) { Cooldown = 0; }
  }

  void ApplySpeedUpgrade(float value)
  {
    ShotSpeed += ShotSpeed * value;
  }
}
