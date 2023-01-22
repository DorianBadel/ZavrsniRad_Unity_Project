using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding_key_controller : MonoBehaviour
{

    //TODO reformat code to make more sense
    //FOUND BUGS -- Key doesnt run away on Q press
    // -- Key doesn't stop following on entering the maze
    private GameObject player;
    private Camera_controller gameMaster;
    private Player_stats playerStats;

    [Header("Requirements")]
    public Transform startingLocation;
    public UnityEngine.AI.NavMeshAgent agent;
    public ParticleSystem destroyEffect;

    [Header("Adjustable variables")]
    public float pickupDistance = 2f;
    public float keySpeed = 0.01f; //does nothing
    public float hidingDistance = 5f;
    public float flyingHeight = 5f; //does nothing

    void Start()
    {
      this.transform.position = startingLocation.position;
      player = GameObject.FindGameObjectWithTag("Player");
      gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<Camera_controller>();
      playerStats = player.GetComponent<Player_stats>();
    }


    void Update()
    {
      Vector3 distanceToPlayer = player.transform.position - this.transform.position;

      if(!playerStats.HasKey && distanceToPlayer.magnitude <= pickupDistance && Input.GetKeyDown(KeyCode.E)){
        Instantiate(destroyEffect,this.transform.position,Quaternion.identity);
        playerStats.PickUpKey();
        gameMaster.keyShouldHide = false;
      }


      if(!gameMaster.keyShouldHide && distanceToPlayer.magnitude >= pickupDistance) MoveToward(player.transform.position);

      if(playerStats.HasKey && Input.GetKeyDown(KeyCode.C)){ //TODO THIS DOES NOT WORK FOR SOME REASON
        playerStats.DropKey();
        gameMaster.ShouldKeyHide(true); //we stopped here
      }


      if(gameMaster.keyShouldHide && CanSeeTarget())
        Hide();
    }

    private void MoveToward(Vector3 target){
      agent.SetDestination(target);
    }

    //TODO reformat code and add rule that the key always hides behind a different tree
    /*private void Hide(){
      float minDistance = Mathf.Infinity;
      Vector3 closestHidingSpot = Vector3.zero;
      Vector3 hidingSpotDir = Vector3.zero;
      GameObject observedObstacle = AllObstacles.World.GetObstacles()[0];

      for(int i = 0; i < AllObstacles.World.GetObstacles().Length; i++){
        Vector3 obstaclesPosition = AllObstacles.World.GetObstacles()[i].transform.position;
        Vector3 hidingSpotDirection = obstaclesPosition - player.transform.position;
        Vector3 hidingSpotPosition = obstaclesPosition + hidingSpotDirection.normalized * hidingDistance;

        if(Vector3.Distance(this.transform.position, hidingSpotPosition) < minDistance){
          closestHidingSpot = hidingSpotPosition;
          hidingSpotDir = hidingSpotDirection;
          observedObstacle = AllObstacles.World.GetObstacles()[i];
          minDistance = Vector3.Distance(this.transform.position,hidingSpotPosition);
        }
      }
      Collider hideCol = observedObstacle.GetComponent<Collider>();
      Ray backRay = new Ray(closestHidingSpot, -hidingSpotDir.normalized);
      RaycastHit info;
      float dist = 250f;
      hideCol.Raycast(backRay,out info, dist);

      agent.SetDestination(info.point + hidingSpotDir.normalized );
    }*/

    private void Hide(){
      Vector3 closestHidingSpot = FindClosestHidingSpot();
      MoveToward(closestHidingSpot);
    }

    private Vector3 FindClosestHidingSpot(){

      float minDistance = Mathf.Infinity;
      Vector3 closestHidingSpot = Vector3.zero;
      GameObject[] obstacles = AllObstacles.World.GetObstacles();

      for(int i = 0; i < obstacles.Length; i++){
        Vector3 obstacleDistance = obstacles[i].transform.position - player.transform.position;
        Vector3 hidingSpotPosition = obstacles[i].transform.position + obstacleDistance.normalized * hidingDistance;

        float distanceToObstacle = Vector3.Distance(this.transform.position, obstacles[i].transform.position)+3f;
        float playersDistanceToObstacle = Vector3.Distance(player.transform.position, obstacles[i].transform.position);

        if(Vector3.Distance(this.transform.position, hidingSpotPosition) < minDistance &&  distanceToObstacle < playersDistanceToObstacle ){
          closestHidingSpot = hidingSpotPosition;
          minDistance = Vector3.Distance(this.transform.position,hidingSpotPosition);
        }
      }

      return closestHidingSpot;

    }

    bool CanSeeTarget(){
      RaycastHit rayHit;
      Vector3 distanceToPlayer = player.transform.position - this.transform.position;

      if(Physics.Raycast(this.transform.position, distanceToPlayer, out rayHit)){
        if(rayHit.transform.gameObject.tag == "Player")
          return true;
      }
      return false;
    }

}
