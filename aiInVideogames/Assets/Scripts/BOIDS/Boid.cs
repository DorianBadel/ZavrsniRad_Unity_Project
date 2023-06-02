using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
  public List<Boid> flock;

  private Vector3 target;

  private FlockMaster FM;
  public float speed;
  private bool outside_limits = false;

  void Start()
  {
    FM = GetComponentInParent<FlockMaster>();
    speed = Random.Range(FM.minSpeed, FM.maxSpeed);

    if (FM.target != null) target = FM.target;
  }

  void Update()
  {
    FindFlock();

    Bounds swimmingBounds = new Bounds(FM.transform.position, FM.swimLimit * 2);
    Vector3 direction = Vector3.zero;

    if (!swimmingBounds.Contains(this.transform.position))
    {
      direction = FM.transform.position - this.transform.position;
      outside_limits = true;
    }
    else
    {
      outside_limits = false;
    }

    if (outside_limits)
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FM.rotationSpeed * Time.deltaTime);
    }
    else
    {
      if (Random.Range(0, 100) < 10)
        speed = Random.Range(FM.minSpeed, FM.maxSpeed);
      if (Random.Range(0, 100) < 20)
        if (flock.Count > 0) ApplyRules();
    }
    transform.Translate(0, 0, Time.deltaTime * speed);
  }

  private void ApplyRules()
  {
    Vector3 separation = Vector3.zero;
    Vector3 alignment = Vector3.zero;
    float cohesion = 0f;
    foreach (Boid fish in flock)
    {
      if (fish == this) return;

      float distance = Vector3.Distance(this.transform.position, fish.transform.position);

      alignment += Alignment(fish, distance);
      separation += Separation(fish, distance);
      cohesion += Cohesion(fish, distance);
    }

    Move(separation, alignment, cohesion);
  }
  //Separation
  private Vector3 Separation(Boid fish, float distance)
  {
    if (distance < FM.separationDistance)
    {
      return this.transform.position - fish.transform.position;
    }
    return Vector3.zero;
  }

  //Alignment
  private Vector3 Alignment(Boid fish, float distance)
  {
    if (distance <= FM.alignmentPull)
    {
      return fish.transform.forward;
    }
    return Vector3.zero;
  }

  //Cohesion
  private float Cohesion(Boid fish, float distance)
  {
    Boid temporaryBoid = fish.GetComponent<Boid>();
    return temporaryBoid.speed;

  }

  private void Move(Vector3 separation, Vector3 alignment, float cohesion)
  {
    alignment = alignment / flock.Count + target - this.transform.position;
    speed = cohesion / flock.Count;

    Vector3 newDirection = (alignment + separation) - this.transform.position;

    if (newDirection != Vector3.zero)
    {
      this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(newDirection), FM.rotationSpeed * Time.deltaTime);
    }

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
