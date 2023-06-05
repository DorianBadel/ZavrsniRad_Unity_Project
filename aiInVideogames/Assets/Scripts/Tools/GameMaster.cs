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
  private UI_Manager uiManager;

  void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerStats = player.GetComponent<PlayerStats>();

    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();
    uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_Manager>();
  }

  void Start()
  {
    PauseGame();
  }

  public void PlayerDropKey(string miniGame)
  {
    if (!playerStats.HasKey) return;

    switch (miniGame)
    {
      case "Maze":
        playerStats.DropKey();
        miniGameController.RespawnKey();
        break;
      case "none":
        playerStats.DropKey();
        ShouldKeyHide(true);
        break;
    }
  }

  public void PlayerPickUpKey(string miniGame)
  {
    if (playerStats.HasKey) return;

    switch (miniGame)
    {
      case "Maze":
        miniGameController.PickUpKey();
        break;
      case "none":
        playerStats.PickUpKey();
        ShouldKeyHide(false);
        break;
    }
  }

  public void OpenMenu()
  {
    PauseGame();
    uiManager.ToggleMenu(UI_Manager.MenuType.InGameMenu, true);
  }

  public void StartGame()
  {
    playerStats.IsInMenu = false;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  public void PauseGame()
  {
    playerStats.IsInMenu = true;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
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
