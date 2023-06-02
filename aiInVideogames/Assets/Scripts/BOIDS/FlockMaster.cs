using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMaster : MonoBehaviour
{
  public Boid[] allBoids;
  public float flockRange = 5f;
  public float separationDistance = 5f;
  public float alignmentDistance = 5f;
  public float alignmentPull = 5f;
  public float cohesionPull = 5f;
  public float separationPull = 5f;

  public Vector3 swimLimit = new Vector3(5, 5, 5);
  public float rotationSpeed = 5f;
  public float minSpeed = 0f;
  public float maxSpeed = 5f;

  [Header("Set target")]
  public Vector3 target;

  void Awake()
  {
    allBoids = FindObjectsOfType<Boid>();
  }

  void OnDrawGizmos()
  {
    // Green
    Gizmos.color = new Color(0.0f, 1.0f, 0.0f);
    Gizmos.DrawWireCube(this.transform.position, swimLimit * 2);
  }
}
