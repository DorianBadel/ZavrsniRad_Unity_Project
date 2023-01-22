using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_controller : MonoBehaviour
{
    private GameObject player;

    [Header("Requirements")]
    public GameObject leftDoorPivot;
    public GameObject rightDoorPivot;

    [Header("Adjustable variables")]
    public float defaultRotationY = 0f;
    public float doorDetectionRange = 10f;
    public float doorSpeed = 15f;

    void Start()
    {
      player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
      OpenTheDoors();
    }

    private void OpenTheDoors(){
      Vector3 rotationY;
      bool playerHasKey = player.GetComponent<Player_stats>().HasKey;
      float distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);

      if(distanceFromPlayer > doorDetectionRange || !playerHasKey){
          rotationY = Vector3.up * doorSpeed * Time.deltaTime; //Close the door
      } else {
          rotationY = -(Vector3.up * doorSpeed * Time.deltaTime); //Open the door
      }

      //Limit the doors hinges
      defaultRotationY -= rotationY.y;
      defaultRotationY = Mathf.Clamp(defaultRotationY, 0f, 120f);

      //Rotate the doors
      leftDoorPivot.transform.localRotation = Quaternion.Euler(0,defaultRotationY,0);
      rightDoorPivot.transform.localRotation = Quaternion.Euler(0,-defaultRotationY,0);
    }
}
