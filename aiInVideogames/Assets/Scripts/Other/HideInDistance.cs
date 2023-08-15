using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInDistance : MonoBehaviour
{
  private GameObject player;
  private GameObject firstChild;

  public float distanceToHide = 500f;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");

    if (this.transform.childCount > 0)
      firstChild = this.transform.GetChild(0).gameObject;
    else Debug.Log("Chunks must have at least one child!");
    StartCoroutine(Hide());
  }

  IEnumerator Hide()
  {
    if (Vector3.Distance(player.transform.position, transform.position) > distanceToHide)
    {
      firstChild.SetActive(false);
    }
    else
    {
      firstChild.SetActive(true);
    }
    yield return new WaitForSeconds(0.1f);
    StartCoroutine(Hide());
  }
}
