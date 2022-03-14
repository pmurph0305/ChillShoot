using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ShotTweener
{
  [SerializeField] public bool TweenIn;
  [SerializeField] public bool TweenOut;
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
    if (idIn != -1) return;
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
    if (idOut != -1) return;
    idOut = t.LeanScale(TweenOutEndScale, tweenOutTime).setEase(easeOut).setFrom(t.localScale).setOnComplete(() =>
    {
      OnTweenCompleted?.Invoke();
      idOut = -1;
    }).uniqueId;
  }

  public float GetTweenOutDuration()
  {
    return TweenOut ? tweenOutTime : 0f;
  }

  public void Cancel()
  {
    LeanTween.cancel(idIn);
    LeanTween.cancel(idOut);
  }

  public void OnGetFromPool()
  {
    idIn = -1;
    idOut = -1;
  }
}
