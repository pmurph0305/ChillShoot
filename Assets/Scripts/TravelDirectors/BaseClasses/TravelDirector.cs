using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TravelDirector : MonoBehaviour, ITravelDirector
{
  [Header("Travel Director")]
  [SerializeField] protected TravelOffsetter offsetter;
  [SerializeField] RotationDirector rotationDirector;
  [SerializeField] protected bool FaceTravelDirection = true;
  [SerializeField] protected bool IncludeOffsetInFaceDirection = true;
  [SerializeField] protected Transform visual;
  [SerializeField] bool FollowGround;
  [SerializeField] LayerMask GroundMask;
  float DistanceFromGround;

  [SerializeField] protected Vector3 travelDirection;
  [SerializeField] protected Vector3 additionalVelocity;
  [SerializeField] protected Vector3 instantVelocity;

  Rigidbody2D rb2d;
  Rigidbody rb;
  private void Awake()
  {
    rb2d = GetComponent<Rigidbody2D>();
    if (rb2d != null && rb2d.isKinematic) { rb2d = null; }
    rb = GetComponent<Rigidbody>();
    if (rb != null && rb.isKinematic) { rb = null; }
  }

  public virtual void ResetTravelDirector()
  {
    travelDirection = GetNewTravelDirection();
    if (offsetter != null)
    {
      offsetter.ResetOffset();
    }
    if (rotationDirector != null)
    {
      rotationDirector.OnGetFromPool();
    }
    if (FaceTravelDirection && visual == null)
    {
      // Debug.LogWarning("No visual transform set, so face direction will not work correctly, but setting to this.transform.");
      visual = this.transform;
    }
    if (FollowGround)
    {
      if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, GroundMask))
      {
        DistanceFromGround = Vector3.Distance(transform.position, hit.point);

      }
    }
    prev = transform.position;
  }

  public void SetAdditionalVelocity(Vector3 velocity)
  {
    additionalVelocity = velocity;
  }

  public void SetInstantVelocity(Vector3 velocity)
  {
    instantVelocity = velocity;
  }

  /// <summary>
  /// Called when the travel director is gotten from the pool, used to calculate a new travelDirection
  /// </summary>
  /// <returns></returns>
  protected abstract Vector3 GetNewTravelDirection();

  /// <summary>
  /// Gets the travel direction, can be overridden to update the travel direction each time it is called.
  /// </summary>
  /// <returns></returns>
  public virtual Vector3 GetTravelDirection()
  {
    return travelDirection;
  }

  /// <summary>
  /// Gets the movement vector for this frame, also rotates the object if there is a rotation director or face travel direction is enabled.
  /// Note that this also increases the offsets timer, so you will need to move the object yourself.
  /// Call UpdateMovement instead if you want this script to move it.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <param name="deltaTime"></param>
  /// <param name="is2d"></param>
  /// <returns></returns>
  public Vector3 GetMovementVector(float movementSpeed, float deltaTime, bool is2d)
  {
    return GetScaledMovement(movementSpeed, deltaTime, is2d);
  }


  /// <summary>
  /// Updates the position of the transform this script is on.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <param name="deltaTime"></param>
  public virtual void UpdateMovement(float movementSpeed, float deltaTime, bool is2d)
  {
    if (rb != null)
    {
      UpdateMovement(rb, movementSpeed, deltaTime);
    }
    else if (rb2d != null)
    {
      UpdateMovement(rb2d, movementSpeed, deltaTime);
    }
    else
    {
      transform.position += GetScaledMovement(movementSpeed, deltaTime, is2d);
    }
    UpdateRotation(deltaTime, is2d);
  }

  protected virtual void UpdateMovement(Rigidbody2D rb2d, float movementSpeed, float deltaTime)
  {
    rb2d.MovePosition(rb2d.position + (Vector2)GetScaledMovement(movementSpeed, deltaTime, true));
  }

  protected virtual void UpdateMovement(Rigidbody rb, float movementSpeed, float deltaTime)
  {
    rb.MovePosition(rb.position + GetScaledMovement(movementSpeed, deltaTime, false));
  }

  RaycastHit hit;
  Vector3 prev;
  Quaternion rotationTarget;
  float rotationSpeed = 90f;
  protected virtual void UpdateRotation(float deltaTime, bool is2d = false)
  {

    if (rotationDirector != null)
    {
      rotationDirector.UpdateTransform(deltaTime);
    }
  }



  Vector3 zero = Vector3.zero;

  /// <summary>
  /// Gets the offset from the offsetter if it is used.
  /// </summary>
  /// <param name="deltaTime"></param>
  /// <returns></returns>
  protected virtual Vector3 GetOffset(float deltaTime)
  {
    return offsetter ? offsetter.GetOffset(deltaTime) : zero;
  }


  /// <summary>
  /// Gets movement vector for this frame.
  /// </summary>
  /// <param name="movementSpeed"></param>
  /// <returns>Movement vector already scaled by time and movementspeed.</returns>
  protected virtual Vector3 GetScaledMovement(float movementSpeed, float deltaTime, bool is2d)
  {
    Vector3 val = deltaTime * movementSpeed * GetTravelDirection();
    Vector3 offset = GetOffset(deltaTime);
    Vector3 instantVel = instantVelocity;
    instantVelocity = Vector3.zero;
    val = val + offset + instantVel + additionalVelocity * deltaTime;
    if (FollowGround)
    {
      if (Physics.Raycast(transform.position + val + offset, Vector3.down, out hit, DistanceFromGround + 0.2f, GroundMask))
      {
        val = (hit.point + Vector3.up * DistanceFromGround) - transform.position;
      }
    }
    if (is2d)
    {
      // visual.rotation = Quaternion.LookRotation(Vector3.forward, val);
      rotationTarget = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, val - (IncludeOffsetInFaceDirection ? zero : offset)), rotationSpeed * deltaTime);
      visual.rotation = Quaternion.LookRotation(Vector3.forward, val - (IncludeOffsetInFaceDirection ? zero : offset));
    }
    else
    {
      rotationTarget = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(val - (IncludeOffsetInFaceDirection ? zero : offset), Vector3.up), rotationSpeed);
      visual.rotation = Quaternion.LookRotation(val - (IncludeOffsetInFaceDirection ? zero : offset));
    }
    return val;
  }
}
