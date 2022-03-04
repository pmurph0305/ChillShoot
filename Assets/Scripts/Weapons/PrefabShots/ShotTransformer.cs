using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DentedPixel;
public class ShotTransformer : MonoBehaviour
{
  [SerializeField] PrefabShot shot;

  [SerializeField] float DelayForTransformation = 0.1f;

  [SerializeField] Vector3 FinalScale = Vector3.zero;
  [SerializeField] LeanTweenType EaseType = LeanTweenType.easeInBack;
  [SerializeField] float TweenTime;

  LTDescr currentTween;

  private void Awake()
  {
    shot.OnCreateAction += OnCreate;
    shot.OnGetFromPoolAction += OnGetFromPool;
    shot.OnReleaseAction += OnRelease;
  }

  private void OnDestroy()
  {
    shot.OnCreateAction -= OnCreate;
    shot.OnGetFromPoolAction -= OnGetFromPool;
    shot.OnReleaseAction -= OnRelease;
  }

  Vector3 originalScale;
  public virtual void OnCreate(PrefabShot s)
  {
    originalScale = transform.localScale;
  }

  public virtual void StartTransformation()
  {
    currentTween = transform.LeanScale(FinalScale, TweenTime).setFrom(transform.localScale).setEase(EaseType).setDelay(DelayForTransformation);
  }



  public virtual void OnRelease(PrefabShot s)
  {
    LeanTween.cancel(currentTween.id);
  }


  public virtual void OnGetFromPool(PrefabShot s)
  {
    transform.localScale = originalScale;
    StartTransformation();
  }
}
