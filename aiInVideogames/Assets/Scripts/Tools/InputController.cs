using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
  MiniGameController miniGameController;

  void Awake()
  {
    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();
  }

  void Update()
  {
    switch (miniGameController.GetActiveMiniGame())
    {
      case "NavMesh":
        HandeNavMeshInputs();
        break;
      case "Maze":
        HandleMazeInputs();
        break;
      case "none":
        HandleDefaultInputs();
        break;
    }
  }

  private void HandeNavMeshInputs()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      GameMaster.Instance.SetActiveMiniGame("none");
    }

    if (Input.GetMouseButtonDown(0))
    {
      SelectUnits.Instance.HandleUnitSelection();
    }

    if (Input.GetMouseButtonDown(1))
    {
      SelectUnits.Instance.HandleUnitCommand();
    }

  }

  private void HandleMazeInputs()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      GameMaster.Instance.SetActiveMiniGame("none");
    }

    if (Input.GetKeyDown(KeyCode.E))
    {
      GameMaster.Instance.PlayerPickUpKey("Maze");
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
      GameMaster.Instance.PlayerDropKey("Maze");
    }

  }

  private void HandleDefaultInputs()
  {
    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
    {
      GameMaster.Instance.OpenMenu();
    }

    if (Input.GetKeyDown(KeyCode.E))
    {
      if (miniGameController.CanPickUpKey())
        GameMaster.Instance.PlayerPickUpKey("none");

      //E is also used for selection boxes
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
      GameMaster.Instance.PlayerDropKey("none");
    }

  }
}
