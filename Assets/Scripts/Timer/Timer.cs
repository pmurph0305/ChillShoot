using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class Timer
{
  public Action OnTimerComplete;
  [SerializeField] float CurrentTime;
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


  /// <summary>
  /// If reset to zero is false, resets to CurrentTime-EndTime so the time carries over to the next "loop"
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
    }
    IsFinished = false;
  }

  public void Reset(float newEndTime)
  {
    CurrentTime = 0;
    EndTime = newEndTime;
    IsFinished = false;
  }




  /// <summary>
  /// returns true when the timer has reached the end
  /// </summary>
  /// <returns></returns>
  public bool Update()
  {
    CurrentTime += Time.deltaTime;
    if (CurrentTime >= EndTime)
    {
      IsFinished = true;
      OnTimerComplete?.Invoke();
    }
    return IsFinished;
  }

  public bool FixedUpdate()
  {
    CurrentTime += Time.fixedDeltaTime;
    if (CurrentTime >= EndTime)
    {
      IsFinished = true;
      OnTimerComplete?.Invoke();
    }
    return IsFinished;
  }
}
