using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
  public enum ActiveMiniGameType
  {
    Maze,
    NavMesh,
    none
  }
  private ActiveMiniGameType activeMiniGame;
  private CameraController cameraController;

  void Awake()
  {
    activeMiniGame = ActiveMiniGameType.none;
    cameraController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CameraController>();
  }

  void Update()
  {
    if (activeMiniGame == ActiveMiniGameType.Maze && cameraController.GetActiveCameraName() != "MazeCamera")
    {
      cameraController.SetActiveCamera("MazeCamera");
    }
    else if (activeMiniGame == ActiveMiniGameType.NavMesh && cameraController.GetActiveCameraName() != "NavMeshCamera")
    {
      cameraController.SetActiveCamera("NavMeshCamera");
    }
    else if (activeMiniGame == ActiveMiniGameType.none && cameraController.GetActiveCameraName() != "DefaultCamera")
    {
      cameraController.SetActiveCamera("DefaultCamera");
    }
  }

  public void SetActiveMiniGame(string miniGame)
  {
    if (miniGame == "maze")
    {
      activeMiniGame = ActiveMiniGameType.Maze;
    }
    else if (miniGame == "navmesh")
    {
      activeMiniGame = ActiveMiniGameType.NavMesh;
    }
    else if (miniGame == "none")
    {
      activeMiniGame = ActiveMiniGameType.none;
    }
  }
}
