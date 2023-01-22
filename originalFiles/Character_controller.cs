using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_controller : MonoBehaviour
{
  private CharacterController charController;
  private float defaultRotationX = 0f;
  private Camera characterCamera;
  private Vector3 fallingVelocity;
  private bool isGrounded;

  [Header("Requirements")]
  public Transform groundCheck;
  public LayerMask groundMask;

  [Header("Adjustable variables | Movement")]
  public float movementSpeed = 10f;
  public float mouseSensitivity = 100f;
  public float jumpStrength = 3f;
  public float groundDetectionRange = 0.4f;
  public float gravity = -9.81f;

  void Start()
  {
    charController = GetComponent<CharacterController>();
    characterCamera = Camera.main; //Preventing the mouse from leaving the window
  }

  void Update()
  {
    if (!GetComponent<Player_stats>().IsDisabled)
    {
      if (GetComponent<Player_stats>().FirstPersonControlls)
      {
        FirstPersonMovement();
      }
      else
      {
        TopDownMovement();
      }
    }
  }

  private void FirstPersonMovement()
  {

    if (!GetComponent<Player_stats>().IsDetected)
    {
      PlayerMovementFP();
      PlayerRotationFP();
      PlayerJumpFP();
    }
    else
    {
      PlayerRotationFP();
    }
  }

  private void TopDownMovement()
  {
    if (this.transform.localRotation != Quaternion.Euler(0, 90, 0))
    {
      this.transform.localRotation = Quaternion.Euler(0, 90, 0);
    }
    Vector3 movement = -transform.right * Input.GetAxis("Vertical") + transform.forward * Input.GetAxis("Horizontal");
    charController.Move(movement * movementSpeed * 2 * Time.deltaTime);
  }

  //FIRST PERSON MOVEMENT
  private void PlayerMovementFP()
  {
    float speed = movementSpeed;
    if (GetComponent<Player_stats>().IsUnderwater)
    {
      speed = speed / 2;
    }
    Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
    charController.Move(movement * speed * Time.deltaTime);
  }

  private void PlayerJumpFP()
  {
    float gravityStrength = gravity;
    if (GetComponent<Player_stats>().IsUnderwater)
    {
      gravityStrength = gravity / 3;
    }
    if (Input.GetButtonDown("Jump") && (isGrounded || GetComponent<Player_stats>().IsUnderwater)) fallingVelocity.y = Mathf.Sqrt(jumpStrength * -2f * gravityStrength);

    //Falling
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDetectionRange, groundMask);
    if (isGrounded && fallingVelocity.y < 0) fallingVelocity.y = -2f;

    fallingVelocity.y += gravityStrength * Time.deltaTime;
    charController.Move(fallingVelocity * Time.deltaTime);
  }

  private void PlayerRotationFP()
  {
    float sensitivity = mouseSensitivity;
    if (GetComponent<Player_stats>().IsUnderwater)
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
}
