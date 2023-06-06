using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{

  VisualElement mainMenu;
  public VisualElement mainMenuContainer, inGameMenuContainer, gameUiContainer, hintContainer;

  public enum MenuType
  {
    MainMenu,
    InGameMenu,
    GameUI,
    Hint
  }

  public enum MessageType
  {
    Success,
    Failure,
    Warning,
    None
  }

  public class ToggleClass
  {
    public string label;
    public bool value;
  }

  void Awake()
  {
  }

  private void OnEnable()
  {
    mainMenu = GetComponent<UIDocument>().rootVisualElement;
    mainMenuContainer = mainMenu.Q("mainMenuContainer");
    inGameMenuContainer = mainMenu.Q<VisualElement>("inGameMenuContainer");
    gameUiContainer = mainMenu.Q<VisualElement>("gameUiContainer");
    hintContainer = mainMenu.Q<VisualElement>("inputHintsContainer");

    AssignButtons();
  }

  private void AssignButtons()
  {
    Button btnStartGame = mainMenu.Q<Button>("btnStartGame");
    Button btnQuitGame = mainMenu.Q<Button>("btnLeaveDesktop");

    Button btnContinueGame = mainMenu.Q<Button>("btnContinue");
    Button btnReturnToMenu = mainMenu.Q<Button>("btnReturnToMenu");
    Button btnQuitGameM = mainMenu.Q<Button>("btnLeaveDesktopM");

    btnStartGame.clicked += () =>
    {
      ToggleMenu(MenuType.MainMenu, false);
      GameMaster.Instance.StartGame();
    };
    btnContinueGame.clicked += () =>
    {
      ToggleMenu(MenuType.InGameMenu, false);
      GameMaster.Instance.StartGame();
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
      case MenuType.Hint:
        if (on)
        {
          hintContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
          hintContainer.style.display = DisplayStyle.None;
        }
        break;
      default:
        break;
    }
  }

  public void DisplayMessage(MessageType type, string message)
  {
    HideAllMessages();
    switch (type)
    {
      case MessageType.Success:
        mainMenu.Q<Label>("SuccessMessage").text = message;
        mainMenu.Q<Label>("SuccessMessage").style.display = DisplayStyle.Flex;
        break;
      case MessageType.Failure:
        mainMenu.Q<Label>("FailureMessage").text = message;
        mainMenu.Q<Label>("FailureMessage").style.display = DisplayStyle.Flex;
        break;
      case MessageType.Warning:
        mainMenu.Q<Label>("WarningMessage").text = message;
        mainMenu.Q<Label>("WarningMessage").style.display = DisplayStyle.Flex;
        break;
      default:
        Debug.Log("WrongMessage type in UI_Manager");
        break;
    }
  }

  private void HideAllMessages()
  {
    mainMenu.Q<Label>("SuccessMessage").style.display = DisplayStyle.None;
    mainMenu.Q<Label>("FailureMessage").style.display = DisplayStyle.None;
    mainMenu.Q<Label>("WarningMessage").style.display = DisplayStyle.None;
  }

  public void DisplayToggles(ToggleClass[] toggles)
  {
    if (toggles.Length > 1)
    {
      Toggle toggle1 = mainMenu.Q<Toggle>("Toggle1");
      Toggle toggle2 = mainMenu.Q<Toggle>("Toggle2");
      toggle1.style.display = DisplayStyle.Flex;
      toggle2.style.display = DisplayStyle.Flex;

      toggle1.label = toggles[0].label;
      toggle1.value = toggles[0].value;
      toggle2.label = toggles[1].label;
      toggle2.value = toggles[1].value;
    }
    else
    {
      Toggle toggle1 = mainMenu.Q<Toggle>("Toggle1");
      Toggle toggle2 = mainMenu.Q<Toggle>("Toggle2");
      toggle1.style.display = DisplayStyle.Flex;
      toggle2.style.display = DisplayStyle.None;

      toggle1.label = toggles[0].label;
      toggle1.value = toggles[0].value;
    }
  }

  public void SetHint(string hint)
  {
    mainMenu.Q<Label>("InputHint").text = hint;
  }

  public bool hintShowing = false;

  public void DisplayHint()
  {
    if (!hintShowing)
    {
      hintShowing = true;
      StartCoroutine(ShowHint());
    }
  }
  IEnumerator ShowHint()
  {
    ToggleMenu(MenuType.Hint, true);
    yield return new WaitForSeconds(4f);
    ToggleMenu(MenuType.Hint, false);
    hintShowing = false;
  }
}
