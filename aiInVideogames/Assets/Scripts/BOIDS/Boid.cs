using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
  public float flockRange = 5f;

  public Boid[] allBoids;
  public List<Boid> flock;
  // Start is called before the first frame update
  void Start()
  {
    allBoids = FindObjectsOfType<Boid>();
  }


  void Reset()
  {
    DebugRays();
  }

  // Update is called once per frame
  void Update()
  {
    DebugRays();
    if (allBoids.Length > 0) FindFlock();
    Separation();

  }
  //Separation
  private void Separation()
  {
    //Average of all neighbours in flock
    Vector3 averagePosition = Vector3.zero;
    foreach (var fish in flock)
    {
      averagePosition += fish.transform.position;
    }
    averagePosition /= flock.Count;

    //Move away from average position
    Vector3 direction = transform.position - averagePosition;
    Debug.DrawRay(transform.position, direction, Color.red);


  }

  //Alignment

  //Cohesion


  private void FindFlock()
  {
    flock.Clear();
    foreach (var fish in allBoids)
    {
      if (fish != this && Vector3.Distance(transform.position, fish.transform.position) < flockRange)
      {
        flock.Add(fish);
      }
    }
  }


  private void DebugRays()
  {
    Vector3 forward = this.transform.TransformDirection(Vector3.forward) * 10;
    Debug.DrawRay(transform.position, forward, Color.green);
  }
}
