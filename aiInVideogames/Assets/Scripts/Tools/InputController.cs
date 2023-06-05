using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
  MiniGameController miniGameController;
  GameMaster gameMaster;

  void Awake()
  {
    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();
    gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
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
      miniGameController.SetActiveMiniGame("none");
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
      miniGameController.SetActiveMiniGame("none");
    }

    if (Input.GetKeyDown(KeyCode.E))
    {
      gameMaster.PlayerPickUpKey("Maze");
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
      gameMaster.PlayerDropKey("Maze");
    }

  }

  private void HandleDefaultInputs()
  {
    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
    {
      gameMaster.OpenMenu();
    }

    if (Input.GetKeyDown(KeyCode.E))
    {
      if (miniGameController.CanPickUpKey())
        gameMaster.PlayerPickUpKey("none");

      //E is also used for selection boxes
    }

    if (Input.GetKeyDown(KeyCode.Q))
    {
      gameMaster.PlayerDropKey("none");
    }

  }
}
