using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
  public bool mazeMiniGameFinished = false, navMeshMiniGameFinished = false, foxKeyCollected = false;

  private GameObject player;
  public bool navmeshWallsMove = false;
  public bool keyShouldHide = true;
  private PlayerStats playerStats;
  private MiniGameController miniGameController;

  void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerStats = player.GetComponent<PlayerStats>();

    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SetActiveMiniGame("none");
    }

    if (playerStats.HasKey && Input.GetKeyDown(KeyCode.Q))
      miniGameController.RespawnKey();
  }

  public void SetActiveMiniGame(string miniGame)
  {
    if (miniGame != "none") playerStats.IsInMiniGame = true;
    else playerStats.IsInMiniGame = false;

    miniGameController.SetActiveMiniGame(miniGame);
  }

  public void ShouldKeyHide(bool answer)
  {
    keyShouldHide = answer;
  }

  public void CompleteMiniGame(string miniGame)
  {
    if (miniGame == "Maze" && playerStats.HasKey)
    {
      miniGameController.CompleteMaze();
      mazeMiniGameFinished = true;
    }
    if (miniGame == "NavMesh")
    {
      miniGameController.CompleteNavMesh();
      navMeshMiniGameFinished = true;
    }
  }
}
