using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChargeSkill
{
  [SerializeField] GameObject chargeObjectVisual;
  [SerializeField] BoxCollider2D box;
  [SerializeField] float MaxChangeTime = 1.0f;
  [SerializeField] float MaxChargeDistance = 5.0f;
  [SerializeField] float CurrentTime;
  [SerializeField] float BaseDamage = 100;
  public void Update(float deltaTime)
  {
    CurrentTime += Time.deltaTime;
    if (CurrentTime > MaxChangeTime)
    {
      CurrentTime = MaxChangeTime;
    }
    UpdateScale(CurrentTime);
  }

  void UpdateScale(float currentTime)
  {
    chargeObjectVisual.transform.localScale = new Vector3(1, currentTime / MaxChangeTime * MaxChargeDistance, 1);
  }

  public void StartCharging(float deltaTime)
  {
    CurrentTime = deltaTime;
    chargeObjectVisual.SetActive(true);
    UpdateScale(CurrentTime);
  }

  [SerializeField] ContactFilter2D filter;
  Collider2D[] overlaps = new Collider2D[0];
  [SerializeField] LayerMask mask;
  EnemyController ec;


  public Vector3 Finish()
  {
    chargeObjectVisual.SetActive(false);
    float d = CurrentTime / MaxChangeTime * MaxChargeDistance;

    // int num = box.OverlapCollider(filter, overlaps);
    overlaps = Physics2D.OverlapBoxAll(chargeObjectVisual.transform.position + chargeObjectVisual.transform.up * d / 2, chargeObjectVisual.transform.lossyScale, chargeObjectVisual.transform.rotation.eulerAngles.z, mask.value);

    Debug.Log("Overlaps:" + overlaps.Length + " angle:" + chargeObjectVisual.transform.rotation.eulerAngles.z + " Scale:" + chargeObjectVisual.transform.lossyScale);
    foreach (var c in overlaps)
    {
      if (EnemyDictionary.ContainsActive(c.transform))
      {
        ec = EnemyDictionary.Get(c.transform);
        ec.OnHitFromAbility(BaseDamage);
      }
      else
      {
        Debug.Log("Not active overlap", c.gameObject);
      }
    }
    // Debug.Log("overlaps:" + overlaps.Length);
    return chargeObjectVisual.transform.position + chargeObjectVisual.transform.up * CurrentTime / MaxChangeTime * MaxChargeDistance;
  }


}
