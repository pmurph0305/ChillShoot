using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePickupPool : PrefabPool<ExperiencePickup>
{
  private static ExperiencePickupPool instance;

  [SerializeField] int DictMultiplier = 10;
  Dictionary<Vector3Int, ExperiencePickup> ActiveDictionary = new Dictionary<Vector3Int, ExperiencePickup>();
  [SerializeField] List<Vector3Int> l = new List<Vector3Int>();
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

  Vector3Int dictPos;
  public void Get(Vector3 position, float expVal)
  {
    dictPos = Vector3Int.RoundToInt(position * DictMultiplier);
    if (ActiveDictionary.ContainsKey(dictPos))
    {
      ActiveDictionary[dictPos].SetValue(ActiveDictionary[dictPos].ExperienceValue + expVal);
    }
    else
    {
      ExperiencePickup p = Get(position);
      p.SetValue(expVal);
      p.SetDictionaryPosition(dictPos);
      ActiveDictionary.Add(dictPos, p);
      l.Add(dictPos);
    }
  }

  protected override void OnReturnedToPool(ExperiencePickup item)
  {
    base.OnReturnedToPool(item);
    // duh, they move.
    if (!ActiveDictionary.ContainsKey(item.DictionaryPosition))
    {
      Debug.LogWarning("Item not in dict L:" + dictPos, item.gameObject);
    }
    else
    {
      ActiveDictionary.Remove(item.DictionaryPosition);
      l.Remove(dictPos);
    }
    // Debug.Log("Count:" + ActiveDictionary.Count);
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
