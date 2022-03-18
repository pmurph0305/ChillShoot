using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WeaponInfo : IGetShotSpawnInfo
{
  [SerializeField] protected WeaponKey _key;
  public WeaponKey key { get { return _key; } private set { _key = value; } }


  [SerializeField] protected float ShotDamage = 10f;
  [SerializeField] protected float CritChance = 5f;
  [SerializeField] protected float CritMultiplier = 1.5f;

  protected float CriticalHitMultiplier()
  {
    if (Random.Range(0f, 100f) < CritChance * PlayerInfo.GetCriticalHitChanceMultiplier())
    {
      return CritMultiplier * PlayerInfo.GetCritcalDamageMultiplier();
    }
    return 1.0f;
  }
  public float GetShotDamage()
  {
    return ShotDamage * PlayerInfo.GetDamageMultiplier() * CriticalHitMultiplier();
  }

  [SerializeField] protected float ShotSpeed = 5f;
  public float GetShotSpeed()
  {
    return ShotSpeed * PlayerInfo.GetWeaponSpeedMultiplier();
  }

  [SerializeField] protected float ShotKnockBack = 0.1f;
  public float GetShotKnockback()
  {
    return ShotKnockBack * PlayerInfo.GetKnockBackMultiplier();
  }

  [SerializeField] protected float ShotLifeTime = 2f;
  public float GetLifeTime()
  {
    return ShotLifeTime * PlayerInfo.GetDurationMultiplier();
  }

  [SerializeField] protected float Cooldown = 0.5f;

  public float GetCooldown()
  {
    return Cooldown * PlayerInfo.GetCooldownMultiplier();
  }

  [SerializeField] protected float ScaleMultiplier = 1.0f;
  public float GetScaleMultiplier()
  {
    return ScaleMultiplier * PlayerInfo.GetAreaMultiplier();
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
  // public void OnHitEnemy(Collider2D Col, EnemyController enemy)
  // {
  //   IWeaponShot s = null;
  //   if (ShotDict.TryGetValue(Col, out s))
  //   {
  //     s.OnHitNewEnemy(enemy);
  //   }
  // }

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
      case WeaponUpgradeType.AdditionalProjectiles:
        ApplyProjectileUpgrade(Mathf.RoundToInt(value));
        break;
      case WeaponUpgradeType.AdditionalHitsBeforeDestroy:
        ApplyAdditionalHitsBeforeDestroy(Mathf.RoundToInt(value));
        break;

    }
  }

  void ApplyProjectileUpgrade(int value)
  {
    ValidSpawnPositions += value;
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

  void ApplyAdditionalHitsBeforeDestroy(int value)
  {
    DestroyAfterXHits += value;
  }

  int currentSpawnIndex = 0;
  public TransformSpawnInfo GetTransformSpawnInfo()
  {
    TransformSpawnInfo info = new TransformSpawnInfo(SpawnLocations[currentSpawnIndex]);
    currentSpawnIndex++;
    if (currentSpawnIndex >= ValidSpawnPositions || currentSpawnIndex >= SpawnLocations.Count)
    {
      currentSpawnIndex = 0;
    }
    return info;
  }

  public int NumberOfShots => ValidSpawnPositions;
  [SerializeField] int ValidSpawnPositions = 1;

  [SerializeField] List<Transform> SpawnLocations = new List<Transform>();
}
