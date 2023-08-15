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

  public void AttemptPickUp()
  {
    if (Vector3.Distance(player.transform.position, this.transform.position) <= pickupDistance)
    {
      PickedUp();
      this.gameObject.SetActive(false);
    }
  }

  public void RespawnKey()
  {
    this.gameObject.SetActive(true);
  }

  private void PickedUp()
  {
    playerStats.PickUpKey();
    Instantiate(destroyEffect, transform.position, Quaternion.identity);
  }
}
