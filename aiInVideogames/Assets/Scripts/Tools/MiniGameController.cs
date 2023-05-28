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

  void Awake()
  {
    activeMiniGame = ActiveMiniGameType.none;
  }

  void Update()
  {
    if (activeMiniGame == ActiveMiniGameType.Maze)
    {
      //do something
      Debug.Log("Maze");
    }
    else if (activeMiniGame == ActiveMiniGameType.NavMesh)
    {
      //do something
    }
    else if (activeMiniGame == ActiveMiniGameType.none)
    {
      //do something
    }
  }
}
