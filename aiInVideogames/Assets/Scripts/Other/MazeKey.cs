using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeKey : MonoBehaviour
{
  private PlayerStats player;

  [Header("Requirements")]
  public ParticleSystem destroyEffect;

  [Header("Adjustable variables")]
  public float pickupDistance = 2f;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
  }

  void Update()
  {
    //TODO seperate this in another script used for all keys
    Vector3 distanceToPlayer = player.transform.position - transform.position;
    bool playerHasKey = player.HasKey;

    if (!playerHasKey && distanceToPlayer.magnitude <= pickupDistance && Input.GetKeyDown(KeyCode.E))
    {
      PickedUp();
      this.gameObject.SetActive(false);
    }

    if (playerHasKey && Input.GetKeyDown(KeyCode.Q))
      player.DropKey();
  }

  public void RespawnKey()
  {
    this.gameObject.SetActive(true);
  }

  private void PickedUp()
  {
    player.PickUpKey();
    Instantiate(destroyEffect, transform.position, Quaternion.identity);
    //Destroy(gameObject);
  }
}
