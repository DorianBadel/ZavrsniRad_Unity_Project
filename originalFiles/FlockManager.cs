using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
  //TODO reformat this code to make more sense
  public GameObject fishPrefab;
  public int numFish = 20;
  public GameObject[] allFish;
  public Vector3 swimLimit = new Vector3(5,5,5);

  public Vector3 goalPos;

  [Header("Fish Settings")]
  [Range(0.0f, 5.0f)] public float minSpeed;
  [Range(0.0f, 5.0f)] public float maxSpeed;
  [Range(1.0f, 10.0f)] public float neighbourDistance;
  [Range(1.0f, 10.0f)] public float personalSpace;
  [Range(0.0f, 5.0f)] public float rotationSpeed;

  void Start()
  {
    allFish = new GameObject[numFish];
    for(int i=0;i<numFish; i++){
      Vector3 pos = this.transform.position +
        new Vector3(Random.Range(-swimLimit.x,swimLimit.x),
                    Random.Range(-swimLimit.y,swimLimit.y),
                    Random.Range(-swimLimit.z,swimLimit.z));

      allFish[i] = (GameObject) Instantiate(fishPrefab,pos,Quaternion.identity);
      allFish[i].GetComponent<Flock>().flockManager = this;
    }
    goalPos = this.transform.position;
  }

  void Update()
  {
    if(Random.Range(0,100)< 3){
      goalPos = this.transform.position + new Vector3(Random.Range(-swimLimit.x,swimLimit.x),
                    Random.Range(-swimLimit.y,swimLimit.y),
                    Random.Range(-swimLimit.z,swimLimit.z));
    }

  }
}
