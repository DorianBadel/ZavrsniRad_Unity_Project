using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMaster : MonoBehaviour
{
  public Boid[] allBoids;
  public float flockRange = 5f;
  public float separationDistance = 5f;
  public float alignmentPull = 5f;

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
}
