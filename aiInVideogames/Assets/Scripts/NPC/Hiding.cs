using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hiding : MonoBehaviour
{
  //TODO: reformat code to make more sense
  //FOUND BUGS -- Key doesnt run away on Q press
  // -- Key doesn't stop following on entering the maze
  private GameObject player;
  private GameMaster gameMaster;
  private PlayerStats playerStats;
  private static GameObject[] obstacles;

  [Header("Requirements")]
  public Transform startingLocation;
  public UnityEngine.AI.NavMeshAgent agent;
  public ParticleSystem destroyEffect;

  [Header("Adjustable variables")]
  public float pickupDistance = 2f;
  public float hidingDistance = 5f;
  public int type = 0;

  void Start()
  {
    obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    this.transform.position = startingLocation.position;
    player = GameObject.FindGameObjectWithTag("Player");
    gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
    playerStats = player.GetComponent<PlayerStats>();
  }


  void Update()
  {
    Vector3 distanceToPlayer = player.transform.position - this.transform.position;

    if (!gameMaster.keyShouldHide && distanceToPlayer.magnitude >= pickupDistance) MoveToward(player.transform.position);

    if (gameMaster.keyShouldHide && CanSeeTarget())
      Hide();
  }

  public bool CanBePickedUp()
  {
    if (Vector3.Distance(player.transform.position, this.transform.position) <= pickupDistance)
    {
      Instantiate(destroyEffect, this.transform.position, Quaternion.identity);
      return true;
    }
    return false;
  }

  private void Hide()
  {
    if (type == 0)
    {
      Vector3 closestHidingSpot = FindClosestHidingSpot();
      MoveToward(closestHidingSpot);
    }
    else
    {
      Vector3 closestHidingSpot = FindClosestHidingSpotUsingKNN();
      MoveToward(closestHidingSpot);
    }
  }

  private void MoveToward(Vector3 target)
  {
    agent.SetDestination(target);
  }
  private bool CanSeeTarget()
  {
    RaycastHit rayHit;
    Vector3 directionToPlayer = player.transform.position - this.transform.position;
    Ray newRay = new Ray(this.transform.position, directionToPlayer);


    if (Physics.Raycast(newRay, out rayHit))
    {
      if (rayHit.transform.gameObject.tag == "Player")
        return true;
    }
    return false;
  }

  private Vector3 FindClosestHidingSpot()
  {

    float minDistance = Mathf.Infinity;
    Vector3 closestHidingSpot = Vector3.zero;

    for (int i = 0; i < obstacles.Length; i++)
    {
      Vector3 hidingSpotPosition = CalculateHidingSpot(i);

      float thisDistanceToObstacle = Vector3.Distance(this.transform.position, obstacles[i].transform.position) + hidingDistance;
      float playersDistanceToObstacle = Vector3.Distance(player.transform.position, obstacles[i].transform.position);

      if (Vector3.Distance(this.transform.position, hidingSpotPosition) < minDistance && thisDistanceToObstacle < playersDistanceToObstacle)
      {
        closestHidingSpot = hidingSpotPosition;
        minDistance = Vector3.Distance(this.transform.position, hidingSpotPosition);
        return closestHidingSpot;
      }
    }
    return closestHidingSpot;
  }

  private Vector3 CalculateHidingSpot(int i)
  {
    Vector3 directionFromPlayer = obstacles[i].transform.position - player.transform.position;
    Debug.DrawRay(obstacles[i].transform.position, directionFromPlayer, Color.blue, 2f);
    return obstacles[i].transform.position + directionFromPlayer.normalized * hidingDistance;
  }

  // //TODO: If you want KNN material
  private Vector3 FindClosestHidingSpotUsingKNN()
  {
    int k = 5; // number of nearest neighbors
    List<Vector3> hidingSpots = new List<Vector3>();
    List<float> distances = new List<float>();

    for (int i = 0; i < obstacles.Length; i++)
    {
      Vector3 obstacleDistance = obstacles[i].transform.position - player.transform.position;
      Vector3 hidingSpotPosition = obstacles[i].transform.position + obstacleDistance.normalized * hidingDistance;
      hidingSpots.Add(hidingSpotPosition);
      distances.Add(Vector3.Distance(this.transform.position, hidingSpotPosition));
    }

    List<int> closestIndices = GetKNearestIndices(distances, k);
    Vector3 closestHidingSpot = Vector3.zero;
    float minDistance = Mathf.Infinity;
    for (int i = 0; i < closestIndices.Count; i++)
    {
      int index = closestIndices[i];
      float distanceToObstacle = Vector3.Distance(this.transform.position, obstacles[index].transform.position) + hidingDistance;
      float playersDistanceToObstacle = Vector3.Distance(player.transform.position, obstacles[index].transform.position);
      if (distances[index] < minDistance && distanceToObstacle < playersDistanceToObstacle)
      {
        closestHidingSpot = hidingSpots[index];
        minDistance = distances[index];
      }
    }

    Debug.DrawRay(closestHidingSpot, Vector3.up, Color.magenta, 2f);

    return closestHidingSpot;
  }

  private List<int> GetKNearestIndices(List<float> distances, int k)
  {
    List<int> indices = Enumerable.Range(0, distances.Count).ToList();
    indices.Sort((a, b) => distances[a].CompareTo(distances[b]));
    return indices.GetRange(0, k);
  }



}
