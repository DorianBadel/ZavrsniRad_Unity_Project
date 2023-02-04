using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
  //TODO all of this code needs reformating

  public FlockManager flockManager;
  private float speed;
  private bool outside_limits = false;

  void Start()
  {
    speed = Random.Range(flockManager.minSpeed,flockManager.maxSpeed);

  }

  void Update()
  {
    Bounds b = new Bounds(flockManager.transform.position, flockManager.swimLimit * 2);

    RaycastHit hit = new RaycastHit();
    Vector3 direction = Vector3.zero;

    if(!b.Contains(transform.position)){ //usefull
      outside_limits = true;
      direction = flockManager.transform.position - transform.position;
    } else if(SeesObstacle(transform.forward)){
      outside_limits = true;
      //forward
      Vector3 newVector = transform.forward;
      Debug.DrawRay(this.transform.position,this.transform.forward * 10, Color.red);

      for(float i=0.1f; i <=1; i+=0.1f){
        Debug.DrawRay(transform.position,Vector3.Lerp(transform.forward,transform.up,i)*10,Color.green);
        Debug.DrawRay(transform.position,Vector3.Lerp(transform.forward,-transform.up,i)*10,Color.green);

        Debug.DrawRay(transform.position,Vector3.Lerp(transform.forward,transform.right,i)*10,Color.green);
        Debug.DrawRay(transform.position,Vector3.Lerp(Vector3.Lerp(transform.forward,transform.up,i),transform.right,i)*10,Color.yellow);
        Debug.DrawRay(transform.position,Vector3.Lerp(transform.forward,-transform.right,i)*10,Color.green);
      }

      direction = Vector3.Reflect(newVector, hit.normal);
    }
    else outside_limits = false;

    if(outside_limits){
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),flockManager.rotationSpeed * Time.deltaTime);

    }else{
      if(Random.Range(0,100)<10)
        speed = Random.Range(flockManager.minSpeed,flockManager.maxSpeed);
      if(Random.Range(0,100)<20)
        ApplyRules();
    }
    transform.Translate(0,0,Time.deltaTime * speed);
  }

  private bool SeesObstacle(Vector3 forward){
    RaycastHit hit = new RaycastHit();

    if(Physics.Raycast(transform.position, forward * 10, out hit)){
      return true;
    }
    else{
      for(int i = 5; i <= 45; i += 5){
        Quaternion R = Quaternion.AngleAxis(-i, new Vector3(0, 1, 0)); //vector3.up
        Vector3 newVector = R * transform.forward ;
        if(Physics.Raycast(transform.position, newVector * 10,out hit)) return true;


        R = Quaternion.AngleAxis(i, new Vector3(0, 1, 0));
        newVector = R * transform.forward ;
        Debug.DrawRay(this.transform.position, newVector * 10, Color.green);

        if(Physics.Raycast(transform.position, newVector * 10,out hit)) return true;

        R = Quaternion.AngleAxis(-i, new Vector3(1, 0, 0)); //vector3.right
        newVector = R * transform.forward ;

        if(Physics.Raycast(transform.position, newVector * 10,out hit)) return true;

        R = Quaternion.AngleAxis(i, new Vector3(1, 0, 0));
        // newVector = R * transform.forward ;
        //
        // if(Physics.Raycast(transform.position, newVector * 10,out hit)) return true;
      }
      return false;
    }
  }

  void ApplyRules(){
    GameObject[] frock;
    frock = flockManager.allFish;

    Vector3 f_centre = Vector3.zero;
    Vector3 n_avoid = Vector3.zero;
    float gr_speed = 0.01f;
    float n_dist;
    int groupSize=0;

    foreach(GameObject fish in frock){
      if(fish != this.gameObject){
        n_dist = Vector3.Distance(fish.transform.position, this.transform.position);
        if(n_dist <= flockManager.neighbourDistance){
          f_centre += fish.transform.position;
          groupSize ++;

          if(n_dist < flockManager.personalSpace){
            n_avoid = n_avoid + (this.transform.position - fish.transform.position);
          }

          Flock anotherFlock = fish.GetComponent<Flock>();
          gr_speed = gr_speed + anotherFlock.speed;
        }
      }
    }

    if(groupSize > 0){
      f_centre = f_centre/groupSize  + flockManager.goalPos - this.transform.position;
      speed = gr_speed/groupSize;

      Vector3 direction = (f_centre+n_avoid) - transform.position;

      if(direction != Vector3.zero)
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), flockManager.rotationSpeed * Time.deltaTime);
    }
  } //end
}
