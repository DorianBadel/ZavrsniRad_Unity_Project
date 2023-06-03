using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInDistance : MonoBehaviour
{
  private GameObject player;

  public float distanceToHide = 500f;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    StartCoroutine(Hide());

  }

  IEnumerator Hide()
  {
    if (Vector3.Distance(player.transform.position, transform.position) > distanceToHide)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.gameObject.SetActive(true);
    }
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(Hide());
  }
}
