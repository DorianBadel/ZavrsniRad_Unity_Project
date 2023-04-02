using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
  public float maxDistance = 100f;

  private FlockMaster FM;

  private bool hasTeleported = false;

  private void Start()
  {
    FM = GetComponentInParent<FlockMaster>();
  }

  private void Update()
  {
    if (!hasTeleported) StartCoroutine(TeleportRandomly());
  }

  IEnumerator TeleportRandomly()
  {
    hasTeleported = true;
    Vector3 randomDirection = Random.insideUnitSphere;
    RaycastHit hit;
    while (Physics.Raycast(transform.position, randomDirection, out hit, maxDistance))
    {
      Debug.DrawRay(transform.position, randomDirection * maxDistance, Color.red);
      randomDirection = Random.insideUnitSphere;
    }
    transform.position = randomDirection * maxDistance;
    yield return new WaitForSeconds(2f);
    hasTeleported = false;
  }

  private Vector3 GetClosestBoid()
  {
    Vector3 closestBoid = Vector3.zero;
    float closestDistance = Mathf.Infinity;
    foreach (var fish in FM.allBoids)
    {
      float distance = Vector3.Distance(transform.position, fish.transform.position);
      if (distance < closestDistance)
      {
        closestDistance = distance;
        closestBoid = fish.transform.position;
      }
    }
    return closestBoid;
  }
}
