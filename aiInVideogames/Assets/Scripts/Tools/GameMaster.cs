using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
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
    // if (topDownCameraNavMesh.enabled && Input.GetKeyDown(KeyCode.Escape))
    // {
    //   FirstPersonCamera();
    // }

    // if (mazeMiniGameActive && Input.GetKeyDown(KeyCode.F))
    // {
    //   if (topDownCameraMaze.enabled) FirstPersonCamera();
    //   else TopDownMazeCamera();
    // }

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

  // public void TopDownMazeCamera()
  // {
  //   topDownCameraMaze.enabled = true;
  //   playerStats.SetFirstPersonControlls(false);
  //   keyShouldHide = true;
  //   playerStats.HasKey = false;
  //   mazeMiniGameActive = true;
  //   Cursor.lockState = CursorLockMode.Locked;
  // }

  // public void TopDownNavMeshCamera()
  // {
  //   topDownCameraMaze.enabled = false;
  //   topDownCameraNavMesh.enabled = true;
  //   playerStats.SetFirstPersonControlls(false);
  //   playerStats.HasKey = false;
  //   playerStats.IsDisabled = true;
  //   mazeMiniGameActive = false;
  //   navmeshWallsMove = true;
  //   Cursor.visible = true;
  //   Cursor.lockState = CursorLockMode.None;
  // }

  // public void FirstPersonCamera()
  // {
  //   topDownCameraMaze.enabled = false;
  //   topDownCameraNavMesh.enabled = false;
  //   playerStats.SetFirstPersonControlls(true);
  //   playerStats.IsDisabled = false; //TODO move to the script that uses it
  //   Cursor.visible = false;
  //   Cursor.lockState = CursorLockMode.Locked;

  // }

  // public void PlayerDetected()
  // {
  //   //Preventing the player to move
  //   playerStats.SetDetected(true);

  //   //Move him to first person camera and prevent AI from moving (for effect)
  //   FirstPersonCamera();
  //   mazeMiniGameActive = false;
  //   //TODO UI info
  //   StartCoroutine("respawn");
  // }

  public bool ShouldMove(int type)
  {
    switch (type)
    {
      case 1:
        return navmeshWallsMove;
      case 2:
        // return mazeMiniGameActive;
        break;
    }
    return false;
  }
}
