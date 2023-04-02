using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

  public Boid[] allBoids;
  public List<Boid> flock;
  public Vector3 calculatedDirection;

  private FlockMaster FM;
  void Start()
  {
    allBoids = FindObjectsOfType<Boid>();
    calculatedDirection = transform.forward;
    FM = GetComponentInParent<FlockMaster>();
  }

  void Update()
  {
    calculatedDirection.Normalize();
    if (allBoids.Length > 0) FindFlock();

    //Apply the 3 rules of Boids
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
    Vector3 averagePosition = Vector3.zero;
    foreach (var fish in flock)
    {
      averagePosition += fish.transform.position;
    }
    averagePosition /= flock.Count;

    Vector3 direction = (transform.position - averagePosition).normalized * FM.separationPull;
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
    Vector3 direction = (transform.forward + averageHeading).normalized * FM.alignmentPull;
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
    Vector3 direction = (averagePosition - transform.position).normalized * FM.cohesionPull;
    Debug.DrawRay(transform.position, direction, Color.yellow);
    calculatedDirection += direction;
  }


  private void FindFlock()
  {
    flock.Clear();
    foreach (var fish in allBoids)
    {
      if (fish != this && Vector3.Distance(transform.position, fish.transform.position) < FM.flockRange)
      {
        flock.Add(fish);
      }
    }
  }
}
