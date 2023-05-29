using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{
  private MiniGameController miniGameController;
  private float movementSpeed = 0.3f;
  CharacterController charController;

  void Awake()
  {
    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();
    charController = GetComponent<CharacterController>();
  }

  void Update()
  {
    if (miniGameController.GetActiveMiniGame() == "Maze")
    {
      Vector3 movement = transform.right * -Input.GetAxis("Vertical") + transform.forward * Input.GetAxis("Horizontal");
      charController.Move(movement * movementSpeed * Time.deltaTime);
    }
  }
}
