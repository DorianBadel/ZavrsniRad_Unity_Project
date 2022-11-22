using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

  private float groundDetectionRange = 0.4f;
  private float gravity = -9.81f;

  private CharacterController charController;
  private Camera characterCamera;
  private PlayerStats stats;

  private Vector3 fallingVelocity;
  private float defaultRotationX = 0f;
  private bool isGrounded = false;

  [Header("Requirements")]
  public Transform feet;
  public LayerMask groundMask;


  void Start()
  {
    charController = GetComponent<CharacterController>();
    stats = GetComponent<PlayerStats>();
    characterCamera = Camera.main;
  }

  void Update()
  {
    if (!stats.IsDisabled)
      if (stats.FirstPersonControlls)
        FirstPersonMovement();
      else PlayerMovementTD();

    //HandleFalling();
  }

  private void HandleFalling()
  {
    isGrounded = Physics.CheckSphere(feet.position, groundDetectionRange, groundMask);

    //if (isGrounded && fallingVelocity.y < 0) fallingVelocity.y = -2f;

    fallingVelocity.y += gravity * Time.deltaTime;
    charController.Move(fallingVelocity * Time.deltaTime);
  }

  private void PlayerMovementTD()
  {
    if (this.transform.localRotation != Quaternion.Euler(0, 90, 0))
      this.transform.localRotation = Quaternion.Euler(0, 90, 0);

    Vector3 movement = -transform.right * Input.GetAxis("Vertical") + transform.forward * Input.GetAxis("Horizontal");

    charController.Move(movement * stats.movementSpeed * 2 * Time.deltaTime);
  }

  private void FirstPersonMovement()
  {
    PlayerRotationFP();

    if (stats.IsDetected) return;

    PlayerMovementFP();
    PlayerJumpFP();
  }

  private void PlayerRotationFP()
  {
    float sensitivity = stats.mouseSensitivity;
    if (stats.IsUnderwater)
    {
      sensitivity = 0.3f * sensitivity;
    }
    //Left - Right rotation
    float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
    this.transform.Rotate(Vector3.up * mouseX);

    //Up - Down rotation
    float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
    defaultRotationX -= mouseY;
    defaultRotationX = Mathf.Clamp(defaultRotationX, -90f, 90f);
    characterCamera.transform.localRotation = Quaternion.Euler(defaultRotationX, 0, 0);
  }

  private void PlayerMovementFP()
  {
    float speed = stats.movementSpeed;
    if (stats.IsUnderwater)
    {
      speed = speed / 2;
    }
    Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
    charController.Move(movement * speed * Time.deltaTime);
  }

  private void PlayerJumpFP()
  {
    float gravityStrength = gravity;
    if (stats.IsUnderwater)
    {
      gravityStrength = gravity / 3;
    }
    if (Input.GetButtonDown("Jump") && (isGrounded || stats.IsUnderwater)) fallingVelocity.y = Mathf.Sqrt(stats.jumpStrength * -2f * gravityStrength);

    //Falling
    HandleFalling();
  }




}
