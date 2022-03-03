using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FpsCounter : MonoBehaviour
{
  [SerializeField] TMP_Text currentText;
  [SerializeField] TMP_Text avgText;

  Queue<float> queue = new Queue<float>();
  [SerializeField] int numberOfFramesToCount = 100;
  // Start is called before the first frame update
  void Start()
  {
    for (int i = 0; i < 100; i++)
    {
      queue.Enqueue(0);
    }
  }

  float current;
  float avg;
  // Update is called once per frame
  void Update()
  {
    queue.Dequeue();
    current = 1 / Time.deltaTime;
    queue.Enqueue(current);
    currentText.SetText(current.ToString("0.00"));
    float total = 0.0f;
    foreach (var item in queue)
    {
      total += item;
    }
    avg = total / queue.Count;
    avgText.text = avg.ToString("0.00");
  }
}
