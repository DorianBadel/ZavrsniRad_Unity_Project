using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWaypoint : MonoBehaviour
{
  private int destinationIndex = 0;
  private MiniGameController miniGameController;
  public MiniGameController.ActiveMiniGameType activeMiniGame = MiniGameController.ActiveMiniGameType.none;

  [Header("Required")]
  public Transform[] waypoints;
  public GameObject surfaceNormal;

  [Header("Adjustable variables")]
  public float moveSpeed = 10f;
  public float stoppingDistance = 0.5f;
  public float rotationSpeed = 3f;


  void Start()
  {
    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();

    if (waypoints.Length <= 1)
    {
      Debug.Log("Game object" + this.name + " doesn't have enough waypoints assigned");
    }
  }


  void Update()
  {
    if (miniGameController.GetActiveMiniGame() == activeMiniGame.ToString())
    {
      MoveToWaypoints();
    }
  }

  private void MoveToWaypoints()
  {
    if (waypoints.Length > 0)
    {

      if (miniGameController.GetActiveMiniGame() == "Maze")
      {
        if (surfaceNormal != null)
        {
          Vector3 directionToTarget = waypoints[destinationIndex].position - transform.position;
          Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, surfaceNormal.transform.up);
          transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
          Quaternion rotateTowards = Quaternion.LookRotation(waypoints[destinationIndex].position - transform.position);
          transform.rotation = Quaternion.Slerp(transform.rotation, rotateTowards, rotationSpeed * Time.deltaTime);
        }
      }

      transform.position = Vector3.MoveTowards(transform.position, waypoints[destinationIndex].position, moveSpeed * Time.deltaTime);

      //Check if has reached waypoint
      if (Vector3.Distance(transform.position, waypoints[destinationIndex].position) < stoppingDistance)
      {
        destinationIndex++;
        if (destinationIndex == waypoints.Length) destinationIndex = 0;
      }
    }
  }

  private void AttachToGround()
  {
    if (surfaceNormal != null)
    {
      Quaternion targetRotation = Quaternion.FromToRotation(transform.up, surfaceNormal.transform.up) * transform.rotation;
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
  }
}
