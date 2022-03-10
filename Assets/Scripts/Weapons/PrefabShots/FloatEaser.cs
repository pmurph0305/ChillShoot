using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FloatEaser
{
  [SerializeField] float start;
  [SerializeField] public float current;
  [SerializeField] float duration;
  [SerializeField] LeanTweenType ease;

  int uniqueId;
  public void StartEase(float end)
  {
    uniqueId = LeanTween.value(start, end, duration).setEase(ease).setOnUpdate((float val) =>
    {
      current = val;
    }).uniqueId;
  }

  public void Stop()
  {
    LeanTween.cancel(uniqueId);
  }
}
