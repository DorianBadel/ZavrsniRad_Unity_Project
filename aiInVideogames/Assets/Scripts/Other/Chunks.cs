using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
  public float distance;

  public GameObject[] dependantObjects;

  public void Activate()
  {
    foreach (GameObject obj in dependantObjects)
    {

      obj.GetComponent<MeshRenderer>().enabled = true;
    }
  }

  public void Deactivate()
  {
    foreach (GameObject obj in dependantObjects)
    {
      obj.GetComponent<MeshRenderer>().enabled = false;
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, new Vector3(500, 10, 500));
  }
}
