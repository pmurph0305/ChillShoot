using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
  [SerializeField] PlayerInfo playerData;
  [SerializeField] ExperienceAttractor expAttractor;
  [SerializeField] float AttractorDuration = 3f;

  [SerializeField] float CurrentHealth = 100f;

  public static Transform PlayerTransform { get; private set; }
  public static Vector3 PlayerVelocity;
  public static Vector3 PlayerDirection;
  public static float PlayerSpeed;
  public static Vector3 PlayerPosition;

  // Start is called before the first frame update
  void Start()
  {
    up = transform.up;
    PlayerTransform = this.transform;
    previous = transform.position;
    CurrentHealth = playerData.pData.MaxHealth;
  }



  Vector3 up;
  Vector3 smoothVel;
  [SerializeField] float smoothTime = 0.01f;


  [Header("Skills")]
  [SerializeField] ChargeSkill chargeSkill;


  [Header("Input Debugging")]
  [SerializeField] float s;
  [SerializeField] Vector3 d;
  [SerializeField] Vector3 v;
  // Update is called once per frame
  [SerializeField] Vector3 input;
  void Update()
  {
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    if (movement.sqrMagnitude > 1f)
    {
      movement = movement.normalized;
    }
    Vector3 initial = transform.position;
    transform.position += playerData.GetPlayerSpeed() * Time.deltaTime * movement;

    // UpdatePlayerParameters(false);
    if (movement.sqrMagnitude > 0.1f)
    {
      // todo: fix quick rotate to opposite movement direction?
      // todo: fix rotating when no input.
      up = Vector3.SmoothDamp(up, movement, ref smoothVel, smoothTime);
      // forward.z = 0;
      transform.rotation = Quaternion.LookRotation(Vector3.forward, up);
    }
    if (Input.GetKeyDown(KeyCode.Space))
    {
      expAttractor.Activate(AttractorDuration);
    }
    HealthRegen(Time.deltaTime);
    UpdatePlayerParameters(movement);

    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      chargeSkill.StartCharging(Time.deltaTime);
    }
    else if (Input.GetKey(KeyCode.LeftShift))
    {
      chargeSkill.Update(Time.deltaTime);
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      Vector3 p = chargeSkill.Finish();
      transform.position = p;
    }
  }


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
