using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
  public GameObject portalPosition;
  public GameObject triggerPosition;

  private bool playerIsInTrigger = false;
  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "unit")
      playerIsInTrigger = true;
  }

  void OnTriggerExit(Collider other)
  {
    if (other.tag == "unit")
      playerIsInTrigger = false;
  }

  void Start()
  {
    StartCoroutine(MovePortal());
  }

  IEnumerator MovePortal()
  {
    if (!playerIsInTrigger)
    {
      Vector3 tempPosition = triggerPosition.transform.position;
      triggerPosition.transform.position = portalPosition.transform.position;
      this.GetComponent<BoxCollider>().center = portalPosition.transform.position;
      portalPosition.transform.position = tempPosition;
    }
    yield return new WaitForSeconds(3f);
    StartCoroutine(MovePortal());
  }
}
