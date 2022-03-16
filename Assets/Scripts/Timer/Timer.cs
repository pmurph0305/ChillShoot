using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Timer
{
  public Action OnTimerComplete;

  [SerializeField] float _CurrentTime;
  public float CurrentTime { get { return _CurrentTime; } private set { _CurrentTime = value; } }
  [SerializeField] float _EndTime;
  public float EndTime { get { return _EndTime; } private set { _EndTime = value; } }
  [SerializeField] bool _IsFinished;
  public bool IsFinished { get { return _IsFinished; } private set { _IsFinished = value; } }

  public float RemainingTime => _EndTime - CurrentTime;

  public float AmountComplete()
  {
    return CurrentTime / EndTime;
  }
  public float AmountComplete01()
  {
    return Mathf.Clamp01(CurrentTime / EndTime);
  }


  public void SetEndTime(float t)
  {
    this.EndTime = t;
  }


  public void SetCurrentTime(float t)
  {
    CurrentTime = t;
  }

  public Timer(float duration)
  {
    CurrentTime = 0.0f;
    EndTime = duration;
  }

  public Timer(float duration, float currentTime)
  {
    CurrentTime = currentTime;
    EndTime = duration;
  }


  /// <summary>
  /// If reset to zero is false, resets to CurrentTime-EndTime so the time carries over to the next "loop"
  /// Note: using false can cause the visual appearance in repeated patterns to be "off" for some reason,
  /// even though it should carry the extra time into the next loop, reseting to zero is more consistent appearing?
  /// </summary>
  /// <param name="resetToZero"></param>
  public void Reset(bool resetToZero = true)
  {
    if (resetToZero)
    {
      CurrentTime = 0;
    }
    else
    {
      CurrentTime = CurrentTime - EndTime;
      if (CurrentTime < 0) { CurrentTime = 0; }
    }
    IsFinished = false;
  }

  public void Reset(float endTime, bool resetToZero = true)
  {
    if (resetToZero)
    {
      CurrentTime = 0;
      EndTime = endTime;
    }
    else
    {
      CurrentTime = CurrentTime - EndTime;
      if (CurrentTime < 0) { CurrentTime = 0; }
      EndTime = endTime;
    }
    IsFinished = false;
  }





  /// <summary>
  /// returns true when the timer has reached the end
  /// </summary>
  /// <returns></returns>
  public bool Update()
  {
    CurrentTime += Time.deltaTime;
    if (CurrentTime > EndTime)
    {
      IsFinished = true;
      OnTimerComplete?.Invoke();
    }
    return IsFinished;
  }

  public bool Update(float deltaTime)
  {
    CurrentTime += deltaTime;
    if (CurrentTime > EndTime)
    {
      IsFinished = true;
      OnTimerComplete?.Invoke();
    }
    return IsFinished;
  }

  public bool FixedUpdate()
  {
    CurrentTime += Time.fixedDeltaTime;
    if (CurrentTime > EndTime)
    {
      IsFinished = true;
      OnTimerComplete?.Invoke();
    }
    return IsFinished;
  }
}
