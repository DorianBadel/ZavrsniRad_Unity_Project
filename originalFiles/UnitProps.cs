using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProps : MonoBehaviour
{

    public Vector3 target;
    public UnityEngine.AI.NavMeshAgent agent;
    public int currentLoad = 0;
    public int maxLoad = 3;

    public GameObject[] deposits;
    public GameObject[] ores;

    public bool wantsToMine = false;
    public bool wantsToDeposit = false;

    public float minDist = 3f;

    void Start()
    {
      target = transform.position;
      deposits = GameObject.FindGameObjectsWithTag("Deposit");
      ores = GameObject.FindGameObjectsWithTag("Ore");
    }

    void Update(){
      if(wantsToMine){
        if(Vector3.Distance(this.transform.position,
        ores[0].transform.position) < minDist){
          if(currentLoad < maxLoad){
            Mine();
          } else goDeposit();
          //TODO fill his load;
          //TODO tell him to deposit if full;
        }
      }
      if(wantsToDeposit){
        if(Vector3.Distance(this.transform.position,
        deposits[0].transform.position) < minDist){
          if(currentLoad > 0){
            Deposit();
          } else if(wantsToMine) goMine();
          //TODO unload his load;
        }
      }
      agent.SetDestination(target);
    }

    private void SetGoal(Vector3 goal){
      target = goal;
    }

    private void Mine(){
      Debug.Log("Im Mining");
      if(currentLoad< maxLoad)
        currentLoad++;
    }

    private void Deposit(){
      Debug.Log("Im Depositing");
      if(currentLoad>0)
        currentLoad--;
    }

    private void goDeposit(){
      wantsToDeposit = true;
      target = deposits[0].transform.position;
    }

    private void goMine(){
      target = ores[0].transform.position;
    }


    //Functions for issued commands
    public void MoveHere(Vector3 goal){
      SetGoal(goal);
      wantsToMine = false;
      wantsToDeposit = false;
    }

    public void MineHere(Vector3 goal){
      if(currentLoad < maxLoad){
        wantsToMine = true;
        SetGoal(goal);
      }else{
        Debug.Log("Storage full");
      }
    }

    public void DepositHere(Vector3 goal){
      if(currentLoad > 0){
        wantsToDeposit = true;
        SetGoal(goal);
      }else{
        Debug.Log("Nothing to deposit");
      }
    }
}
