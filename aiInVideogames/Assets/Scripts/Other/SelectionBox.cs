using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
  private bool playerIsInTrigger = false;
  private GameMaster gameMaster;

  public enum SelectionType
  {
    Maze,
    MazeCompletion,
    NavMesh
  }

  public SelectionType selectionType;

  private void Start()
  {
    gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMaster>();
  }

  private void OnTriggerEnter(Collider collider)
  {
    if (collider.CompareTag("Player") && selectionType != SelectionType.MazeCompletion ||
    collider.CompareTag("MazePlayer") && selectionType == SelectionType.MazeCompletion)
    {
      playerIsInTrigger = true;
    }
  }
  private void OnTriggerExit(Collider collider)
  {
    if (collider.CompareTag("Player") || collider.CompareTag("MazePlayer"))
    {
      playerIsInTrigger = false;
    }
  }

  private void Update()
  {
    if (playerIsInTrigger)
    {
      if (selectionType == SelectionType.MazeCompletion)
      {
        gameMaster.CompleteMiniGame("Maze");
      }
      if (Input.GetKeyDown(KeyCode.E))
      {
        switch (selectionType)
        {
          case SelectionType.Maze:
            gameMaster.SetActiveMiniGame("Maze");
            break;
          case SelectionType.NavMesh:
            gameMaster.SetActiveMiniGame("NavMesh");
            break;
        }
      }
    }
  }
}
