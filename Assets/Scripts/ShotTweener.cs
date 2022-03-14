using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ShotTweener
{
  [SerializeField] bool TweenIn;
  [SerializeField] bool TweenOut;
  [SerializeField] LeanTweenType easeOut = LeanTweenType.easeInBack;
  [SerializeField] LeanTweenType easeIn = LeanTweenType.easeOutBack;
  [SerializeField] Vector3 StartScale = Vector3.zero;
  [SerializeField] Vector3 TweenOutEndScale = Vector3.zero;
  [SerializeField] float tweenInTime = 0.2f;
  [SerializeField] float tweenOutTime = 0.2f;
  int idIn = -1;
  int idOut = -1;
  public void StartTweenIn(Transform t, Vector3 toScale, Action OnTweenCompleted)
  {
    if (!TweenIn)
    {
      OnTweenCompleted?.Invoke(); return;
    }
    idIn = t.LeanScale(toScale, tweenInTime).setEase(easeIn).setFrom(StartScale).setOnComplete(() =>
    {
      OnTweenCompleted?.Invoke();
      idIn = -1;
    }).uniqueId;
  }

  public void StartTweenOut(Transform t, Action OnTweenCompleted)
  {
    if (!TweenOut)
    {
      OnTweenCompleted?.Invoke(); return;
    }
    idOut = t.LeanScale(TweenOutEndScale, tweenOutTime).setEase(easeOut).setFrom(t.localScale).setOnComplete(OnTweenCompleted).uniqueId;
  }


  public void Cancel()
  {
    LeanTween.cancel(idIn);
    LeanTween.cancel(idOut);
  }
}
