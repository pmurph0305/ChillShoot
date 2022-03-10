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
    forward = transform.up;
    PlayerTransform = this.transform;
  }

  public void TakeDamage(float damage)
  {
    Health -= damage;
  }

  Vector3 forward;
  Vector3 smoothVel;
  [SerializeField] float smoothTime;
  // Update is called once per frame
  void Update()
  {
    Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
    Vector3 initial = transform.position;
    transform.position += Speed * Time.deltaTime * movement;
    PlayerVelocity = (transform.position - initial) / Time.deltaTime;
    PlayerDirection = PlayerVelocity.normalized;
    PlayerSpeed = PlayerVelocity.magnitude;

    PlayerPosition = transform.position;
    if (movement.sqrMagnitude > 0.1f)
    {
      // todo: fix quick rotate to opposite movement direction?
      forward = Vector3.SmoothDamp(forward, movement, ref smoothVel, smoothTime);
      // forward.z = 0;
      transform.rotation = Quaternion.LookRotation(Vector3.forward, forward);
      // Vector3 f = transform.rotation * Vector3.up;
      // Debug.DrawLine(transform.position, transform.position + f * 5, Color.green, 10f);
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      expAttractor.Activate(AttractorDuration);
    }
  }
  [SerializeField] float AttractorDuration = 3f;
}
