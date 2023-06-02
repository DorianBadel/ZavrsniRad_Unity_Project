using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
  public enum DoorType
  {
    CastleDoor,
    PoolDoor
  }
  public DoorType doorType;
  private GameMaster gameMaster;
  private GameObject player;

  private Quaternion initialRotation;
  private Quaternion openRotation;
  private bool doorIsOpening = false;

  void Awake()
  {
    gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
    player = GameObject.FindGameObjectWithTag("Player");
  }

  void Start()
  {
    initialRotation = this.transform.rotation;
    openRotation = Quaternion.Euler(0, initialRotation.y + 180, 0);
  }

  void FixedUpdate()
  {
    if (this.transform.rotation == openRotation | doorIsOpening) return;
    switch (doorType)
    {
      case DoorType.CastleDoor:
        if (gameMaster.foxKeyCollected)
        {
          UnlockedDoorOpen();
        }
        break;
      case DoorType.PoolDoor:
        if (gameMaster.mazeMiniGameFinished && gameMaster.navMeshMiniGameFinished)
        {
          UnlockedDoorOpen();
        }
        break;
      default:
        break;
    }

  }

  void UnlockedDoorOpen()
  {
    if (Vector3.Distance(this.transform.position, player.transform.position) < 5)
    {
      OpenDoor();
    }
  }

  private void OpenDoor()
  {
    doorIsOpening = true;
    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, openRotation, Time.deltaTime * 2);
    doorIsOpening = false;
  }
}
