using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class TextDisplayer : MonoBehaviour, IPoolable<TextDisplayer>
{
  [SerializeField] RectTransform _rect;
  [SerializeField] TMP_Text text;
  [SerializeField] float DisplayTime = 1.0f;

  [SerializeField] ScaleTweener tweener;

  public void OnCreate()
  {
    text.text = "";
  }

  public void OnGetFromPool()
  {
    tweener.StartTween(_rect, () =>
    {
      Release();
    });
    transform.LeanMove(transform.position + new Vector3(Random.Range(-1, 1), Random.Range(0.5f, 1), 0), tweener.TweenInTime);
  }

  public void Release()
  {
    this.gameObject.SetActive(false);
  }

  private void OnDisable()
  {
    _pool.Release(this);
  }

  public IObjectPool<TextDisplayer> _pool;
  public void SetPool(IObjectPool<TextDisplayer> pool)
  {
    _pool = pool;
  }

  public void SetText(string value)
  {
    text.text = value;
  }




}
