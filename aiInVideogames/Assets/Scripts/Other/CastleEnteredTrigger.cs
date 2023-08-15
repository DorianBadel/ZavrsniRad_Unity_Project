using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleEnteredTrigger : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    if (other.tag == "Player")
    {
      GameMaster.Instance.CastleEnteredTrigger();
    }
  }
}
