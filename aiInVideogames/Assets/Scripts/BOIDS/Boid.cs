using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
  public float flockRange = 5f;

  public Boid[] allBoids;
  public List<Boid> flock;
  public Vector3 calculatedDirection;

  public float separationPull = 5f;
  public float alignmentPull = 5f;
  public float cohesionPull = 5f;
  void Start()
  {
    allBoids = FindObjectsOfType<Boid>();
    calculatedDirection = transform.forward;
  }


  void Reset()
  {
    DebugRays();
  }

  // Update is called once per frame
  void Update()
  {
    calculatedDirection.Normalize();
    DebugRays();
    if (allBoids.Length > 0) FindFlock();
    Separation();
    Alignment();
    Cohesion();

    Debug.DrawRay(transform.position, calculatedDirection, Color.magenta);

    transform.position += calculatedDirection * Time.deltaTime;
    transform.rotation = Quaternion.LookRotation(calculatedDirection);

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
    Vector3 direction = (transform.position - averagePosition).normalized * separationPull;
    Debug.DrawRay(transform.position, direction, Color.red);
    calculatedDirection += direction;


  }

  //Alignment
  private void Alignment()
  {
    Vector3 averageHeading = Vector3.zero;
    foreach (var fish in flock)
    {
      averageHeading += fish.transform.forward;
    }
    averageHeading /= flock.Count;
    Vector3 direction = (transform.forward + averageHeading).normalized * alignmentPull;
    Debug.DrawRay(transform.position, direction, Color.blue);
    calculatedDirection += direction;


  }

  //Cohesion
  private void Cohesion()
  {
    Vector3 averagePosition = Vector3.zero;
    foreach (var fish in flock)
    {
      averagePosition += fish.transform.position;
    }
    averagePosition /= flock.Count;
    Vector3 direction = (averagePosition - transform.position).normalized * cohesionPull;
    Debug.DrawRay(transform.position, direction, Color.yellow);
    calculatedDirection += direction;
  }


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
