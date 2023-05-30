using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeKey : MonoBehaviour
{
  private PlayerStats playerStats;
  private GameObject player;

  [Header("Requirements")]
  public ParticleSystem destroyEffect;

  [Header("Adjustable variables")]
  public float pickupDistance = 2f;

  void Start()
  {
    playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    player = GameObject.FindGameObjectWithTag("MazePlayer");
  }

  void Update()
  {
    //TODO seperate this in another script used for all keys
    Vector3 distanceToPlayer = player.transform.position - transform.position;
    bool playerHasKey = playerStats.HasKey;

    if (!playerHasKey && distanceToPlayer.magnitude <= pickupDistance && Input.GetKeyDown(KeyCode.E))
    {
      PickedUp();
      this.gameObject.SetActive(false);
    }
  }

  public void RespawnKey()
  {
    playerStats.DropKey();
    this.gameObject.SetActive(true);
  }

  private void PickedUp()
  {
    playerStats.PickUpKey();
    Instantiate(destroyEffect, transform.position, Quaternion.identity);
  }
}
