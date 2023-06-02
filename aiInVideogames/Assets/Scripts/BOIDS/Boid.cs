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
  }

  void Update()
  {
    FindFlock();
    target = FM.target;
    Vector3 direction = Vector3.zero;

    Bounds swimmingBounds = new Bounds(FM.transform.position, FM.swimLimit * 2);
    if (LeavingBounds())
    {
      direction = FM.transform.position - this.transform.position;
      direction.y = Random.Range(-FM.swimLimit.y + 1, FM.swimLimit.y - 1);
      outside_limits = true;
      // Debug.DrawRay(transform.position, direction.normalized * 5, Color.red, 0.5f);
    }
    else
    {
      outside_limits = false;
    }

    if (outside_limits)
    {
      this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FM.rotationSpeed * Time.deltaTime);
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

  private bool LeavingBounds()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, 2))
    {
      if (hit.collider.gameObject.tag != "Fish")
      {
        return true;
      }
    }
    if (Physics.Raycast(transform.position, Quaternion.AngleAxis(15, transform.up) * transform.forward, out hit, 1))
    {
      if (hit.collider.gameObject.tag != "Fish")
      {
        return true;
      }
    }
    if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-15, transform.up) * transform.forward, out hit, 1))
    {
      if (hit.collider.gameObject.tag != "Fish")
      {
        return true;
      }
    }
    if (Physics.Raycast(transform.position, Quaternion.AngleAxis(15, transform.right) * transform.forward, out hit, 1))
    {
      if (hit.collider.gameObject.tag != "Fish")
      {
        return true;
      }
    }
    if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-15, transform.right) * transform.forward, out hit, 1))
    {
      if (hit.collider.gameObject.tag != "Fish")
      {
        return true;
      }
    }
    return false;
  }



  private void ApplyRules()
  {
    Vector3 alignment = Vector3.zero;
    Vector3 separation = Vector3.zero;
    float cohesion = 0f;

    foreach (Boid fish in flock)
    {
      if (fish == this) return;

      float distance = Vector3.Distance(this.transform.position, fish.transform.position);

      alignment += Alignment(fish, distance);
      separation += Separation(fish, distance);
      cohesion += Cohesion(fish, distance);
    }

    Move(separation * FM.separationPull, alignment * FM.alignmentPull, cohesion * FM.cohesionPull);
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
    if (distance <= FM.alignmentDistance)
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

    Debug.DrawRay(transform.position, alignment.normalized * 5, Color.green, 1);
    Debug.DrawRay(transform.position, separation.normalized * 5, Color.blue, 1);

    Vector3 newDirection = (alignment + separation);

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
