using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWaypoint : MonoBehaviour
{
  //TODO pretvori type=1 u ENUM, izračun rotateTowards nemora postojati ako type nije 1
    private int destinationIndex = 0;
    private GameObject gameMaster;
    public int type = 1;

    [Header("Required")]
    public Transform[] waypoints;

    [Header("Adjustable variables")]
    public float moveSpeed = 10f;
    public float stoppingDistance = 0.5f;
    public float rotationSpeed = 3f;

    void Start()
    {
      gameMaster = GameObject.FindGameObjectWithTag("GameController");
      if(waypoints.Length <= 1) {
        Debug.Log("Game object" + this.name + " doesn't have enough waypoints assigned");
      }
    }


    void Update()
    {
      if(gameMaster.GetComponent<Camera_controller>().ShouldMove(type)){
        MoveToWaypoints();
      }

    }

    private void MoveToWaypoints(){
      if(waypoints.Length > 0){
        Quaternion rotateTowards = Quaternion.LookRotation(waypoints[destinationIndex].position - transform.position);

        if(type != 1){
            transform.rotation = Quaternion.Slerp(transform.rotation,rotateTowards,rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[destinationIndex].position ,moveSpeed * Time.deltaTime);
        //Check if has reached waypoint
        if(Vector3.Distance(transform.position,waypoints[destinationIndex].position) < stoppingDistance){
          destinationIndex++;
          if(destinationIndex == waypoints.Length) destinationIndex = 0;
        }
      }
    }
}
