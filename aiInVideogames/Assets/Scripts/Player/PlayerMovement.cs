using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  private CharacterController charController;
  private PlayerStats stats;
  private Vector3 fallingVelocity;
  //private float defaultRotationX = 0f;
  private bool isGrounded = false;
  private float groundDetectionRange = 0.4f;
  private float gravity = -9.81f;

  [Header("Requirements")]
  public Transform feet;
  public LayerMask groundMask;

  void Start()
  {
    charController = GetComponent<CharacterController>();
    stats = GetComponent<PlayerStats>();
  }

  // Update is called once per frame
  void Update()
  {
    TopDownMovement();
    HandleFalling();
  }

  private void TopDownMovement()
  {
    if (this.transform.localRotation != Quaternion.Euler(0, 90, 0))
      this.transform.localRotation = Quaternion.Euler(0, 90, 0);

    Vector3 movement = -transform.right * Input.GetAxis("Vertical") + transform.forward * Input.GetAxis("Horizontal");

    charController.Move(movement * stats.movementSpeed * 2 * Time.deltaTime);
  }

  private void HandleFalling()
  {
    isGrounded = Physics.CheckSphere(feet.position, groundDetectionRange, groundMask);

    // if (isGrounded && fallingVelocity.y < 0) fallingVelocity.y = -2f;

    fallingVelocity.y += gravity * Time.deltaTime;
    charController.Move(fallingVelocity * Time.deltaTime);


  }


}
