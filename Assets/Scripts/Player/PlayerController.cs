using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class PlayerController : MonoBehaviour
{
  [SerializeField] PlayerInfo playerData;
  [SerializeField] ExperienceAttractor expAttractor;
  [SerializeField] float AttractorDuration = 3f;

  [SerializeField] float CurrentHealth = 100f;

  public static Vector3 PlayerVelocity;
  public static Vector3 PlayerDirection;
  public static float PlayerSpeed;
  public static Vector3 PlayerPosition;

  // Start is called before the first frame update
  void Start()
  {
    up = transform.up;
    previous = transform.position;
    CurrentHealth = playerData.pData.MaxHealth;
  }



  Vector3 up;
  Vector3 smoothVel;
  [SerializeField] float smoothTime = 0.01f;


  [Header("Skills")]
  [SerializeField] protected ChargeSkill chargeSkill;


  [Header("Input Debugging")]
  [SerializeField] float s;
  [SerializeField] Vector3 d;
  [SerializeField] Vector3 v;
  // Update is called once per frame
  [SerializeField] protected Vector3 input;
  void Update()
  {
    OnProcessInput();
    transform.position += playerData.GetPlayerSpeed() * Time.deltaTime * input;

    // UpdatePlayerParameters(false);
    if (input.sqrMagnitude > 0.1f)
    {
      // todo: fix quick rotate to opposite movement direction?
      // todo: fix rotating when no input.
      up = Vector3.SmoothDamp(up, input, ref smoothVel, smoothTime);
      // forward.z = 0;
      transform.rotation = Quaternion.LookRotation(Vector3.forward, up);
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      expAttractor.Activate(AttractorDuration);
    }
    HealthRegen(Time.deltaTime);
    UpdatePlayerParameters(input);
  }

  protected abstract void OnProcessInput();


  public bool TakeDamage(float damage)
  {
    CurrentHealth -= damage;
    if (CurrentHealth < 0)
    {
      return true;
    }
    return false;
  }

  void HealthRegen(float deltaTime)
  {
    CurrentHealth += playerData.GetPlayerHealthRegen() * deltaTime;
    if (CurrentHealth > playerData.pData.MaxHealth) CurrentHealth = playerData.pData.MaxHealth;
  }

  Vector3 previous;
  void UpdatePlayerParameters(Vector3 movement)
  {
    PlayerDirection = movement;
    PlayerSpeed = movement != Vector3.zero ? movement.magnitude * playerData.GetPlayerSpeed() : 0;
    PlayerVelocity = transform.up * PlayerSpeed;
    PlayerPosition = transform.position;
    s = PlayerSpeed;
    v = PlayerVelocity;
    d = PlayerDirection;
    input = movement;
  }

}
