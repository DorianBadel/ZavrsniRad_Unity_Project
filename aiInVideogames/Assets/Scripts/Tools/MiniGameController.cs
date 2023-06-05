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
  private GameMaster gameMaster;

  private bool playerDetected = false;
  private MazeKey mazeKey;
  private bool completeCoroutineRunning = false;

  [Header("[Maze] Requirements")]
  public Transform mazeRespawnPoint;
  public GameObject mazeKeyPrefab;

  [Header("[NavMesh] Requirements")]
  public GameObject platformPrefab;
  public RTSBots botScript;

  [Header("[Other] Requirements")]
  public Hiding hidingWithKey;


  void Awake()
  {
    // Getting controllers
    activeMiniGame = ActiveMiniGameType.none;
    cameraController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CameraController>();
    gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();

    // Getting maze key
    mazeKey = GameObject.FindGameObjectWithTag("MazeKey").GetComponent<MazeKey>();

    // Checking if maze respawn point is set
    if (mazeRespawnPoint == null) Debug.LogError("Maze respawn point not set");

  }

  void Update()
  {
    if (activeMiniGame == ActiveMiniGameType.Maze && cameraController.GetActiveCameraName() != "MazeCamera")
    {
      ShowCursor(false);
      cameraController.SetActiveCamera("MazeCamera");
    }
    else if (activeMiniGame == ActiveMiniGameType.NavMesh && cameraController.GetActiveCameraName() != "NavMeshCamera")
    {
      ShowCursor(true);
      cameraController.SetActiveCamera("NavMeshCamera");
    }
    else if (activeMiniGame == ActiveMiniGameType.none && cameraController.GetActiveCameraName() != "DefaultCamera")
    {
      ShowCursor(false);
      cameraController.SetActiveCamera("DefaultCamera");
    }
  }

  private void ShowCursor(bool show)
  {
    if (show)
    {
      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = true;
    }
    else
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    }
  }

  public void SetActiveMiniGame(string miniGame)
  {
    if (playerDetected)
    {
      Debug.Log("Player detected, can't change mini game");
      return;
    }

    if (miniGame == "Maze")
    {
      activeMiniGame = ActiveMiniGameType.Maze;
    }
    else if (miniGame == "NavMesh")
    {
      activeMiniGame = ActiveMiniGameType.NavMesh;
    }
    else if (miniGame == "none")
    {
      activeMiniGame = ActiveMiniGameType.none;
    }
  }

  public string GetActiveMiniGame()
  {
    return activeMiniGame.ToString();
  }

  // Maze mini game

  public bool GetPlayerDetected()
  {
    return playerDetected;
  }

  public void SetPlayerDetected(GameObject player, bool detected)
  {
    if (activeMiniGame == ActiveMiniGameType.Maze)
    {
      gameMaster.SetActiveMiniGame("none");
      playerDetected = detected;
    }
    StartCoroutine(ResetPlayerDetected(player));
  }

  IEnumerator ResetPlayerDetected(GameObject player)
  {
    yield return new WaitForSeconds(2);
    player.transform.position = mazeRespawnPoint.position;
    RespawnKey();
    playerDetected = false;
  }

  public void PickUpKey()
  {
    mazeKey.AttemptPickUp();
  }

  public void RespawnKey()
  {
    mazeKey.RespawnKey();
  }

  public bool CanPickUpKey()
  {
    return hidingWithKey.CanBePickedUp();
  }

  public void CompleteMaze()
  {
    if (completeCoroutineRunning) return;

    StartCoroutine(MazeCompleteCoroutine());
  }

  IEnumerator MazeCompleteCoroutine()
  {
    completeCoroutineRunning = true;
    GameObject player = GameObject.FindGameObjectWithTag("MazePlayer");
    MazeCompleteAnimation(player);
    yield return new WaitForSeconds(2);
    player.transform.position = mazeRespawnPoint.position;
    RespawnKey();
    gameMaster.SetActiveMiniGame("none");
    completeCoroutineRunning = false;
  }

  public void MazeCompleteAnimation(GameObject player)
  {
    GameObject mazeCompletionObj = GameObject.Instantiate(mazeKeyPrefab, player.transform.position, Quaternion.identity);
    GameObject mazeExit = GameObject.FindGameObjectWithTag("MazeExit");

    StartCoroutine(MazeCompleteAnimationCoroutine(mazeCompletionObj, mazeExit));
  }

  IEnumerator MazeCompleteAnimationCoroutine(GameObject mazeCompletionObj, GameObject mazeExit)
  {
    float t = 0;
    while (t < 2)
    {
      t += Time.deltaTime;
      mazeCompletionObj.transform.position = Vector3.Lerp(mazeCompletionObj.transform.position, mazeExit.transform.position, t);
      yield return null;
    }
    Destroy(mazeCompletionObj);
  }

  public void CompleteNavMesh()
  {
    if (completeCoroutineRunning) return;

    StartCoroutine(NavMeshCompleteCoroutine());
  }

  IEnumerator NavMeshCompleteCoroutine()
  {
    //MovePresure plate
    Vector3 newPosition = new Vector3(platformPrefab.transform.position.x, platformPrefab.transform.position.y, platformPrefab.transform.position.z - platformPrefab.transform.localScale.z);
    platformPrefab.transform.position = Vector3.Lerp(platformPrefab.transform.position, newPosition, 0.5f);
    //Display message on screen
    yield return new WaitForSeconds(2);
    botScript.Reset();
    gameMaster.SetActiveMiniGame("none");
    Vector3 oldPosition = new Vector3(platformPrefab.transform.position.x, platformPrefab.transform.position.y, platformPrefab.transform.position.z + platformPrefab.transform.localScale.z);
    platformPrefab.transform.position = Vector3.Lerp(platformPrefab.transform.position, newPosition, 0.5f);
    completeCoroutineRunning = false;
  }
}
