using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_key_controller : MonoBehaviour
{
    private GameObject player;

    [Header("Requirements")]
    public ParticleSystem destroyEffect;

    [Header("Adjustable variables")]
    public float pickupDistance = 2f;

    void Start()
    {
      player = GameObject.FindGameObjectWithTag("Player");
    }

    //TODO add respawn capability

    void Update()
    {
      //TODO seperate this in another script used for all keys
      Vector3 distanceToPlayer = player.transform.position - transform.position;
      bool playerHasKey = player.GetComponent<Player_stats>().HasKey;

      if(!playerHasKey && distanceToPlayer.magnitude <= pickupDistance && Input.GetKeyDown(KeyCode.E)){
          PickedUp();
          this.gameObject.SetActive(false);
      }

      if(playerHasKey && Input.GetKeyDown(KeyCode.Q))
        player.GetComponent<Player_stats>().DropKey();
    }

    public void RespawnKey(){
      this.gameObject.SetActive(true);
    }

    private void PickedUp(){
      player.GetComponent<Player_stats>().PickUpKey();
      Instantiate(destroyEffect,transform.position,Quaternion.identity);
      //Destroy(gameObject);
    }

}
