using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
  private FlockMaster FM;
  public Vector3 swimLimit = new Vector3(5, 5, 5);
  private Vector3 initialPos;

  private void Start()
  {
    FM = GetComponentInParent<FlockMaster>();
    swimLimit = FM.swimLimit;
    FM.target = this.transform.position;
    initialPos = this.transform.position;
  }

  void Update()
  {
    if (Random.Range(0, 100) < 3)
    {
      this.transform.position = initialPos + new Vector3(Random.Range(-swimLimit.x, swimLimit.x),
                    Random.Range(-swimLimit.y, swimLimit.y),
                    Random.Range(-swimLimit.z, swimLimit.z));

      FM.target = this.transform.position;
    }

  }
}
