using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickupPool : PrefabPool<ExperiencePickup>
{
  private static ExperiencePickupPool instance;

  private void Start()
  {
    if (instance != null && instance != this)
    {
      Debug.LogWarning("Duplicate experience pickup pool", this.gameObject);
      Debug.LogError("other duplicate:", instance.gameObject);
    }
    instance = this;
  }

  public static void GetExpItem(Vector3 position, float expVal) => instance.Get(position, expVal);
  public void Get(Vector3 position, float expVal)
  {
    ExperiencePickup p = Get(position);
    p.SetValue(expVal);
  }

  protected override ExperiencePickup CreatePooledItem()
  {
    ExperiencePickup ep = base.CreatePooledItem();
    CollisionDictionary.Add(ep.GetCollider, ep);
    return ep;
  }

  Dictionary<Collider2D, ExperiencePickup> CollisionDictionary = new Dictionary<Collider2D, ExperiencePickup>();

  public static bool TryGetActiveExperiencePickup(Collider2D col, out ExperiencePickup val) => instance.TryGetExperiencePickup(col, out val);
  private bool TryGetExperiencePickup(Collider2D col, out ExperiencePickup val)
  {
    // we don't want to return true for non-active exp pickups
    if (CollisionDictionary.TryGetValue(col, out val))
    {
      if (val.isActiveAndEnabled)
      {
        return true;
      }
      return false;
    }
    return false;
  }
}
