using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
  public bool mazeMiniGameFinished = false, navMeshMiniGameFinished = false, foxKeyCollected = false;

  private GameObject player;
  public static GameMaster Instance { get; private set; }
  public bool navmeshWallsMove = false;
  public bool keyShouldHide = true;
  private PlayerStats playerStats;
  private MiniGameController miniGameController;
  private UI_Manager uiManager;

  private UI_Manager.ToggleClass[] foxKeyToggle, castleToggles;

  void Awake()
  {
    Instance = this;
    player = GameObject.FindGameObjectWithTag("Player");
    playerStats = player.GetComponent<PlayerStats>();

    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();
    uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UI_Manager>();
  }

  void Start()
  {
    PauseGame();
  }

  void OnEnable()
  {
    foxKeyToggle = new UI_Manager.ToggleClass[1] {
      new UI_Manager.ToggleClass { label = "Get key from fox", value = false }
    };
    castleToggles = new UI_Manager.ToggleClass[2] {
      new UI_Manager.ToggleClass { label = "Get key from picture frame labyrinth", value = false },
      new UI_Manager.ToggleClass { label = "Get key from table mini game", value = false }
    };

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
        keyShouldHide = true;
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
        foxKeyCollected = true;
        keyShouldHide = false;
        foxKeyToggle[0].value = true;
        uiManager.DisplayMessage(UI_Manager.MessageType.Success, "You found the key! Now you can enter the castle!");
        uiManager.DisplayToggles(foxKeyToggle);
        break;
    }
  }

  public void OpenMenu()
  {
    PauseGame();
    uiManager.ToggleMenu(UI_Manager.MenuType.GameUI, false);
    uiManager.ToggleMenu(UI_Manager.MenuType.InGameMenu, true);
  }

  public void StartGame()
  {
    playerStats.IsInMenu = false;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;

    uiManager.ToggleMenu(UI_Manager.MenuType.GameUI, true);
    uiManager.DisplayMessage(UI_Manager.MessageType.Warning, "One of the hiding foxes has the key to the castle, find it and grab it to enter the castle!");
    uiManager.DisplayToggles(foxKeyToggle);
  }

  public void PauseGame()
  {
    playerStats.IsInMenu = true;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public void SetActiveMiniGame(string miniGame)
  {
    if (miniGame != "none")
    {
      playerStats.IsInMiniGame = true;
      if (miniGame == "Maze")
      {
        uiManager.DisplayMessage(UI_Manager.MessageType.Warning, "You have to find the key in the labyrinth, and bring it to the rightmost point of the painting!");
        DisplayHint("Use WASD to move, and E to pick up the key.");
      }
      else if (miniGame == "NavMesh")
      {
        uiManager.DisplayMessage(UI_Manager.MessageType.Warning, "You have to command the glass to gather the liquid from one jar and carry it to the other jar! Once the second jar is full you will complete the mini game!");
        DisplayHint("Left click on the glass to select it, right click to issue a command to it.");
      }
    }
    else
    {
      playerStats.IsInMiniGame = false;
      uiManager.ToggleMenu(UI_Manager.MenuType.GameUI, true);
    }

    miniGameController.SetActiveMiniGame(miniGame);
  }

  public void DisplayHint(string hint)
  {
    uiManager.SetHint(hint);
    uiManager.DisplayHint();
  }

  public void CompleteMiniGame(string miniGame)
  {
    if (miniGame == "Maze")
    {
      miniGameController.CompleteMaze();
      uiManager.DisplayMessage(UI_Manager.MessageType.Success, "You have completed the maze mini game! Congratulations!");
      mazeMiniGameFinished = true;
    }
    if (miniGame == "NavMesh")
    {
      miniGameController.CompleteNavMesh();
      uiManager.DisplayMessage(UI_Manager.MessageType.Success, "You have completed the navigation mesh mini game! Congratulations!");
      navMeshMiniGameFinished = true;
    }
    DisplayCastleToggles();
  }

  public void CastleEnteredTrigger()
  {
    keyShouldHide = true;
    foxKeyCollected = false;
    playerStats.DropKey();
    uiManager.DisplayMessage(UI_Manager.MessageType.Success, "Congratulations, now you have to solve two puzzles. They are on the first floor of the castle.");
    DisplayCastleToggles();
  }

  private void DisplayCastleToggles()
  {
    if (mazeMiniGameFinished && navMeshMiniGameFinished)
    {
      uiManager.DisplayMessage(UI_Manager.MessageType.Success, "You have completed all the mini games, you can now enter the last door!");
      uiManager.ToggleMenu(UI_Manager.MenuType.GameUI, false);
    }
    castleToggles[0].value = mazeMiniGameFinished;
    castleToggles[1].value = navMeshMiniGameFinished;

    uiManager.DisplayToggles(castleToggles);
  }
}
