using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ScaleTweener
{
  [SerializeField] LeanTweenType easeOut = LeanTweenType.easeOutBack;
  [SerializeField] LeanTweenType easeIn = LeanTweenType.easeInCubic;

  [SerializeField] float tweenInTime = 0.2f;
  [SerializeField] float tweenOutTime = 0.2f;

  [SerializeField] float timeBetweenTweens = 0.2f;
  [SerializeField] Vector3 tweenInToScale = Vector3.one;
  [SerializeField] Vector3 tweenOutToScale = Vector3.zero;

  int idIn = -1;
  int idOut = -1;
  public void StartTween(RectTransform r, Action OnTweenCompleted)
  {
    idIn = r.LeanScale(tweenInToScale, tweenInTime).setEase(easeIn).setFrom(tweenOutToScale).uniqueId;
    idOut = r.LeanScale(tweenOutToScale, tweenOutTime).setEase(easeOut).setFrom(tweenInToScale).setDelay(timeBetweenTweens).setOnComplete(OnTweenCompleted).uniqueId;
  }

  public void StartTween(Transform t, Action OnTweenCompleted)
  {
    idIn = t.LeanScale(tweenInToScale, tweenInTime).setEase(easeIn).setFrom(tweenOutToScale).uniqueId;
    idOut = t.LeanScale(tweenOutToScale, tweenOutTime).setEase(easeOut).setFrom(tweenInToScale).setDelay(timeBetweenTweens).setOnComplete(OnTweenCompleted).uniqueId;
  }

  public void Cancel()
  {
    LeanTween.cancel(idIn);
    LeanTween.cancel(idOut);
  }
}
