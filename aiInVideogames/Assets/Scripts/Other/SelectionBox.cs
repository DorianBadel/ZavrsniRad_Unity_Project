using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
  private bool playerIsInTrigger = false;
  // Start is called before the first frame update
  private void OnTriggerEnter(Collider collider)
  {
    if (collider.CompareTag("Player"))
    {
      playerIsInTrigger = true;
    }
  }
  private void OnTriggerExit(Collider collider)
  {
    if (collider.CompareTag("Player"))
    {
      playerIsInTrigger = false;
    }
  }

  private void onGUI()
  {
    if (playerIsInTrigger)
    {
      GUI.Label(new Rect(10, 10, 100, 20), "Press E to interact");
    }
  }

  private void Update()
  {
    if (playerIsInTrigger)
    {
      if (Input.GetKeyDown(KeyCode.E))
      {
        //do something
        Debug.Log("Interacted with object");
      }
    }
  }
}
