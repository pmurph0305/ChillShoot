using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerController : MonoBehaviour
{
  [SerializeField] ExperienceAttractor expAttractor;

  [SerializeField] float AttractorDuration = 3f;

  public static Transform PlayerTransform { get; private set; }
  public static Vector3 PlayerVelocity;
  public static Vector3 PlayerDirection;
  public static float PlayerSpeed;

  public static Vector3 PlayerPosition;


  [SerializeField] float Experience = 0;

  [Header("Speed")]
  [SerializeField] float BaseSpeed = 1.0f;
  [SerializeField] float Speed = 1.0f;

  [Header("HP")]
  [SerializeField] float BaseHealth = 100;
  [SerializeField] float MaxHealth = 100;
  float CurrentHealth = 100;



  public void GainExp(float value)
  {
    Experience += value;
    LevelUp.OnExperienceChanged(Experience);
  }

  #region upgrades
  private void Awake()
  {
    OnHealthUpgrade += OnHealthUpgradeHandler;
    OnSpeedUpgrade += OnSpeedUpgradeHandler;
  }

  public static Action<float> OnHealthUpgrade;
  void OnHealthUpgradeHandler(float value)
  {
    float currentPercent = CurrentHealth / MaxHealth;
    MaxHealth += BaseHealth * value;
    CurrentHealth = MaxHealth * currentPercent;
  }

  public static Action<float> OnSpeedUpgrade;
  void OnSpeedUpgradeHandler(float value)
  {
    // Debug.Log("?");
    Speed += BaseSpeed * value;
  }
  #endregion



  // Start is called before the first frame update
  void Start()
  {
    up = transform.up;
    PlayerTransform = this.transform;
    previous = transform.position;
  }

  public void TakeDamage(float damage)
  {
    CurrentHealth -= damage;
    if (CurrentHealth < 0)
    {
      Debug.LogError("dead");
    }
  }

  Vector3 up;
  Vector3 smoothVel;
  [SerializeField] float smoothTime = 0.01f;


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
    transform.position += Speed * Time.deltaTime * movement;

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

    UpdatePlayerParameters(movement);
  }

  Vector3 previous;
  void UpdatePlayerParameters(Vector3 movement)
  {
    PlayerDirection = movement;
    PlayerSpeed = movement != Vector3.zero ? movement.magnitude * Speed : 0;
    PlayerVelocity = transform.up * PlayerSpeed;
    PlayerPosition = transform.position;
    s = PlayerSpeed;
    v = PlayerVelocity;
    d = PlayerDirection;
    input = movement;
  }

}
