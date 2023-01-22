using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
  private GameObject player;
  public bool mazeEnemiesMove = true;
  public bool navmeshWallsMove = false;
  public bool keyShouldHide = true;
  private PlayerStats playerStats;

  [Header("Requirements")]
  public Camera topDownCameraMaze;
  public Camera topDownCameraNavMesh;
  public Transform mazeRespawnPoint;
  private GameObject mazeKey;

  void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    playerStats = player.GetComponent<PlayerStats>();

    mazeKey = GameObject.FindGameObjectWithTag("MazeKey");
  }

  void Update()
  {
    if (topDownCameraNavMesh.enabled && Input.GetKeyDown(KeyCode.Escape))
    {
      FirstPersonCamera();
    }

    if (mazeEnemiesMove && Input.GetKeyDown(KeyCode.F))
    {
      if (topDownCameraMaze.enabled) FirstPersonCamera();
      else TopDownMazeCamera();
    }
  }

  public void ShouldKeyHide(bool answer)
  {
    keyShouldHide = answer;
  }

  public void TopDownMazeCamera()
  {
    topDownCameraMaze.enabled = true;
    playerStats.SetFirstPersonControlls(false);
    keyShouldHide = true;
    playerStats.HasKey = false;
    mazeEnemiesMove = true;
    Cursor.lockState = CursorLockMode.Locked;
  }

  public void TopDownNavMeshCamera()
  {
    topDownCameraMaze.enabled = false;
    topDownCameraNavMesh.enabled = true;
    playerStats.SetFirstPersonControlls(false);
    playerStats.HasKey = false;
    playerStats.IsDisabled = true;
    mazeEnemiesMove = false;
    navmeshWallsMove = true;
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
  }

  public void FirstPersonCamera()
  {
    topDownCameraMaze.enabled = false;
    topDownCameraNavMesh.enabled = false;
    playerStats.SetFirstPersonControlls(true);
    playerStats.IsDisabled = false; //TODO move to the script that uses it
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

  }

  public void PlayerDetected()
  {
    //Preventing the player to move
    playerStats.SetDetected(true);

    //Move him to first person camera and prevent AI from moving (for effect)
    FirstPersonCamera();
    mazeEnemiesMove = false;
    //TODO UI info
    StartCoroutine("respawn");
  }

  public bool ShouldMove(int type)
  {
    switch (type)
    {
      case 1:
        return navmeshWallsMove;
      case 2:
        return mazeEnemiesMove;
    }
    return false;
  }

  IEnumerator respawn()
  {
    yield return new WaitForSeconds(3f);
    player.transform.position = mazeRespawnPoint.position;
    yield return new WaitForSeconds(0.1f);
    playerStats.SetDetected(false);
    mazeKey.GetComponent<MazeKey>().RespawnKey();
    TopDownMazeCamera();
    //TODO Remove hasKey, respawn the key if it was picked up
  }
}
