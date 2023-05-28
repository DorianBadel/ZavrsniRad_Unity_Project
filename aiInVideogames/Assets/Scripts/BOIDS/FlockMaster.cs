using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMaster : MonoBehaviour
{
  public Boid[] allBoids;
  public float flockRange = 5f;
  public float separationPull = 5f;
  public float alignmentPull = 5f;
  public float cohesionPull = 5f;


  public float targetPull = 5f;

  [Header("Set target")]
  public Transform target;

  void Awake()
  {
    allBoids = FindObjectsOfType<Boid>();
  }
}
