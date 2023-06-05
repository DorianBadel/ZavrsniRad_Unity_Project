using UnityEngine;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{

  VisualElement mainMenu;
  GameMaster gameMaster;

  public VisualElement mainMenuContainer, inGameMenuContainer, gameUiContainer;

  public enum MenuType
  {
    MainMenu,
    InGameMenu,
    GameUI
  }

  void Awake()
  {
    gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
  }

  private void OnEnable()
  {
    mainMenu = GetComponent<UIDocument>().rootVisualElement;
    mainMenuContainer = mainMenu.Q("mainMenuContainer");
    inGameMenuContainer = mainMenu.Q<VisualElement>("inGameMenuContainer");
    gameUiContainer = mainMenu.Q<VisualElement>("gameUiContainer");

    Button btnStartGame = mainMenu.Q<Button>("btnStartGame");
    Button btnQuitGame = mainMenu.Q<Button>("btnLeaveDesktop");

    Button btnContinueGame = mainMenu.Q<Button>("btnContinue");
    Button btnReturnToMenu = mainMenu.Q<Button>("btnReturnToMenu");
    Button btnQuitGameM = mainMenu.Q<Button>("btnLeaveDesktopM");

    // Label successMessage = mainMenu.Q<Label>("successMessage");
    // Label failureMessage = mainMenu.Q<Label>("failureMessage");
    // Toggle firstToggle = mainMenu.Q<Toggle>("Toggle1");
    // Toggle secondToggle = mainMenu.Q<Toggle>("Toggle2");

    btnStartGame.clicked += () =>
    {
      ToggleMenu(MenuType.MainMenu, false);
      gameMaster.StartGame();
    };
    btnContinueGame.clicked += () =>
    {
      ToggleMenu(MenuType.InGameMenu, false);
      gameMaster.StartGame();
    };
    btnReturnToMenu.clicked += () =>
    {
      ToggleMenu(MenuType.InGameMenu, false);
      ToggleMenu(MenuType.MainMenu, true);
    };
    btnQuitGame.clicked += () =>
    {
      Debug.Log("Game is quitting");
      Application.Quit();
    };
    btnQuitGameM.clicked += () =>
    {
      Debug.Log("Game is quitting");
      Application.Quit();
    };
  }

  public void ToggleMenu(MenuType type, bool on)
  {
    switch (type)
    {
      case MenuType.MainMenu:
        if (on)
        {
          mainMenuContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
          mainMenuContainer.style.display = DisplayStyle.None;
        }
        break;
      case MenuType.InGameMenu:
        if (on)
        {
          inGameMenuContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
          inGameMenuContainer.style.display = DisplayStyle.None;
        }
        break;
      case MenuType.GameUI:
        if (on)
        {
          gameUiContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
          gameUiContainer.style.display = DisplayStyle.None;
        }
        break;
      default:
        break;
    }
  }
}
