using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExperiencePickup : MonoBehaviour, IPoolable<ExperiencePickup>
{
  private IObjectPool<ExperiencePickup> _pool;
  [SerializeField] SpriteRenderer spriteRenderer;

  private Material m;

  [SerializeField] ColorList colorList;

  public float speed = 2f;

  int ColorProperty = Shader.PropertyToID("_Color");
  int EmissionProperty = Shader.PropertyToID("_EmissionColor");

  public void OnCreate()
  {
    m = spriteRenderer.material;
  }

  public void OnGetFromPool()
  {

  }

  public void Release()
  {
    SetTarget(null);
    this.gameObject.SetActive(false);
  }

  private void OnDisable()
  {
    _pool.Release(this);
  }

  public void SetPool(IObjectPool<ExperiencePickup> pool)
  {
    _pool = pool;
  }
  [SerializeField] private float ExpVal;
  public float ExperienceValue => ExpVal;


  [Header("Debug")]
  [SerializeField] Color color;
  [SerializeField, ColorUsageAttribute(false, true)] Color prop;

  public void SetValue(float value)
  {
    ExpVal = value;
    // spriteRenderer.color = colorList.GetColorForValue(value);
    m.color = colorList.GetColorForValue(value);
    color = colorList.GetColorForValue(value);
    prop = colorList.GetEmissionForValue(value);
    m.SetVector(EmissionProperty, colorList.GetEmissionForValue(value));
  }

  void SetColor()
  {

  }

  Vector3Int _DictionaryPosition = Vector3Int.zero;
  public Vector3Int DictionaryPosition => _DictionaryPosition;
  public void SetDictionaryPosition(Vector3Int pos)
  {
    _DictionaryPosition = pos;
  }

  Transform target;
  public void SetTarget(Transform target)
  {
    this.target = target;
  }

  private void Update()
  {
    if (target != null)
    {
      transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
    }
  }
}
