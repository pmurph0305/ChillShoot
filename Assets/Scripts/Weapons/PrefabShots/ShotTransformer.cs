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
  [SerializeField] bool isTweening = false;

  int tweenId;
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

  [SerializeField] ScaleTweener scaleTweener;
  public virtual void StartTransformation()
  {
    // I believe this is the correct way to do it..
    tweenId = transform.LeanScale(FinalScale, TweenTime).setFrom(originalScale).setEase(EaseType).setDelay(DelayForTransformation).uniqueId;
    // scaleTweener.StartTween(this.transform, () => { });
  }



  public virtual void OnRelease(PrefabShot s)
  {
    LeanTween.cancel(tweenId);
    // scaleTweener.Cancel();
  }


  public virtual void OnGetFromPool(PrefabShot s)
  {
    isTweening = false;
    transform.localScale = originalScale;
    StartTransformation();
  }
}
