using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelDirector : MonoBehaviour
{
  [SerializeField] protected Vector3 travelDirection;

  public virtual void OnGetFromPool(PrefabShot shot)
  {
    travelDirection = GetNewTravelDirection(shot.weaponInfo);
  }

  protected abstract Vector3 GetNewTravelDirection(WeaponInfo info);

  public virtual Vector3 GetTravelDirection()
  {
    return travelDirection;
  }
}
