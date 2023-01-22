using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingUnderWater : MonoBehaviour
{
  public Player_stats playerStats;

  void Awake(){
    playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_stats>();
  }

  public void OnTriggerEnter(){
    playerStats.IsUnderwater = true;
  }

  public void OnTriggerExit(){
    playerStats.IsUnderwater = false;
  }
}
