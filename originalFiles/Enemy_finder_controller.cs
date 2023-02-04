using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_finder_controller : MonoBehaviour
{
  //This code has been checked no further changes needed

    private GameObject player;
    private Camera_controller gameMaster;
    private Player_stats playerStats;

    [Header("Required")]
    public Light lamp;

    [Header("Adjustable variables detection")]
    public float detectionAngle = 90f;
    public float detectionRange = 8;

    void Start()
    {
      gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<Camera_controller>();

      player = GameObject.FindGameObjectWithTag("Player");
      playerStats = player.GetComponent<Player_stats>();

    }

    void Update()
    {
      if(playerStats.IsDetected) {
        //Rotate towards player if he is detected
        Quaternion targetRot = Quaternion.LookRotation(player.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRot, 30f*Time.deltaTime);
      }

      if(gameMaster.mazeEnemiesMove){
        //If this part of the game is relevant
        DetectPlayer();
      }

    }

    private void DetectPlayer(){

      //Draw a ray towards the player
      Vector3 directionToPlayer = player.transform.position - transform.position;
      RaycastHit objectHit;
      Ray newRay = new Ray(transform.position,directionToPlayer);

      //Calculate at what angle the player is compared to the AI-s forward direction
      float angleToPlayer = Vector3.Angle(directionToPlayer,transform.forward);


      //If he is within the detection cone angle, and isn't too far check if the ray hit a player
      if(angleToPlayer < detectionAngle/2 && Physics.Raycast(newRay,out objectHit, detectionRange)){
        if(objectHit.collider.gameObject.CompareTag("Player")){
          gameMaster.PlayerDetected();

          //Just for visuals
          lamp.color = Color.red;
          InvokeRepeating("FlickerLampToLight",1f,0.4f);
          InvokeRepeating("FlickerLampToDark",1.3f,0.4f);
        }
      } else lamp.color = Color.white;
    }

    private void FlickerLampToLight(){
      lamp.color = new Color(0.8f,0,0,1f);
    }

    private void FlickerLampToDark(){
      lamp.color =Color.red;

    }


}