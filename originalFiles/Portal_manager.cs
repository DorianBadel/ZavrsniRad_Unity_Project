using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_manager : MonoBehaviour
{
    private GameObject player;
    private Player_stats playerStats;
    private Transform destination;

    void Start(){
      player = GameObject.FindGameObjectWithTag("Player");
      playerStats = player.GetComponent<Player_stats>();
    }


    //TODO find a way to avoid this overload mby don't its fine
    public void teleportTo(Transform destination){
      if(playerStats.HasKey){
        this.destination = destination;
        StartCoroutine("teleportWithDelay");
      }
    }

    public void teleportTo(Transform destination,bool allowedToTravel){
      this.destination = destination;
      StartCoroutine("teleportWithDelay");
    }

    IEnumerator teleportWithDelay(){
      playerStats.IsDisabled = true;
      yield return new WaitForSeconds(0.1f);
      player.transform.position = destination.position;
      yield return new WaitForSeconds(0.1f);
      playerStats.IsDisabled = false;
    }
}
