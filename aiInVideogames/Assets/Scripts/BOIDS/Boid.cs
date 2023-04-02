using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
  //TODO
  //Add a force towards a target
  //Add a force away from walls
  public List<Boid> flock;
  public Vector3 calculatedDirection;

  private Transform target = null;

  private FlockMaster FM;
  void Start()
  {
    calculatedDirection = transform.forward;
    FM = GetComponentInParent<FlockMaster>();

    if (FM.target != null) target = FM.target;
  }

  void Update()
  {
    calculatedDirection.Normalize();
    if (FM.allBoids.Length > 0) FindFlock();

    //Apply the 3 rules of Boids
    Separation();
    Alignment();
    Cohesion();

    Move();

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

  private void Move()
  {
    Vector3 movingDirection = Vector3.zero;
    if (target != null)
    {
      movingDirection = (target.position - transform.position).normalized * FM.targetPull;
    }

    movingDirection += calculatedDirection;

    Debug.DrawRay(transform.position, movingDirection, Color.magenta);

    transform.position += movingDirection * Time.deltaTime;
    transform.rotation = Quaternion.LookRotation(movingDirection);

  }


  private void FindFlock()
  {
    flock.Clear();
    foreach (var fish in FM.allBoids)
    {
      if (fish != this && Vector3.Distance(transform.position, fish.transform.position) < FM.flockRange)
      {
        flock.Add(fish);
      }
    }
  }
}
