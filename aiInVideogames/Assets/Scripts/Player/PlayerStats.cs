using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

  [Header("Movement | Adjustable variables")]
  public float movementSpeed = 10f;
  public float mouseSensitivity = 100f;
  public float jumpStrength = 3f;
  public bool HasKey = false;
  public bool IsUnderwater = false;
  public bool IsInMiniGame = false;

  public void PickUpKey()
  {
    HasKey = true;
  }

  public void DropKey()
  {
    HasKey = false;
  }
}
