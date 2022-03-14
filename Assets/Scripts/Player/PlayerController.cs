using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] ExperienceAttractor expAttractor;
  public static Transform PlayerTransform { get; private set; }

  public static Vector3 PlayerVelocity;
  public static Vector3 PlayerDirection;
  public static float PlayerSpeed;

  public static Vector3 PlayerPosition;
  [SerializeField] float Speed = 1.0f;
  [SerializeField] float Health = 100;

  [SerializeField] float Experience = 0;

  public void GainExp(float value)
  {
    Experience += value;
  }

  // Start is called before the first frame update
  void Start()
  {
    up = transform.up;
    PlayerTransform = this.transform;
    previous = transform.position;
  }

  public void TakeDamage(float damage)
  {
    Health -= damage;
  }

  Vector3 up;
  Vector3 smoothVel;
  [SerializeField] float smoothTime;

  Vector3 smoothingVel;

  [SerializeField] float s;
  [SerializeField] Vector3 d;
  [SerializeField] Vector3 v;
  // Update is called once per frame
  void Update()
  {
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
    Vector3 initial = transform.position;
    transform.position += Speed * Time.deltaTime * movement;

    // UpdatePlayerParameters(false);
    if (movement.sqrMagnitude > 0.1f)
    {
      // todo: fix quick rotate to opposite movement direction?
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
    PlayerSpeed = movement != Vector3.zero ? Speed : 0;
    PlayerVelocity = transform.up * PlayerSpeed;
    PlayerPosition = transform.position;
    s = PlayerSpeed;
    v = PlayerVelocity;
    d = PlayerDirection;
  }
  [SerializeField] float AttractorDuration = 3f;
}
