using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
  private GameObject target;
  private MiniGameController miniGameController;

  [Header("Required")]
  public Light lamp;

  [Header("Adjustable variables detection")]
  public float detectionAngle = 90f;
  public float detectionRange = 8;

  void Start()
  {
    miniGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MiniGameController>();

    target = GameObject.FindGameObjectWithTag("MazePlayer");

  }

  void Update()
  {
    if (miniGameController.GetPlayerDetected())
    {
      Quaternion targetRot = Quaternion.LookRotation(target.transform.position - this.transform.position);
      this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRot, 30f * Time.deltaTime);

      lamp.color = Color.red;
      InvokeRepeating("FlickerLampToLight", 1f, 0.4f);
      InvokeRepeating("FlickerLampToDark", 1.3f, 0.4f);
    }
    else
    {
      CancelInvoke(); lamp.color = Color.white;
    }

    if (miniGameController.GetActiveMiniGame() == "Maze")
    {
      //If this part of the game is relevant
      LookForPlayer();
      lamp.range = detectionRange;
      lamp.innerSpotAngle = detectionAngle;
    }

  }

  private void LookForPlayer()
  {
    //Draw a ray towards the player
    Vector3 directionToPlayer = target.transform.position - transform.position;
    RaycastHit objectHit;
    Ray newRay = new Ray(transform.position, directionToPlayer);
    Debug.DrawRay(transform.position, directionToPlayer, Color.red);

    //Calculate at what angle the player is compared to the AI-s forward direction
    float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);

    Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.green);

    //If he is within the detection cone angle, and isn't too far check if the ray hit a player
    if (angleToPlayer < detectionAngle / 2 && Physics.Raycast(newRay, out objectHit, detectionRange))
    {
      if (objectHit.collider.gameObject.CompareTag("MazePlayer"))
      {
        miniGameController.SetPlayerDetected(target, true);
      }
    }
  }

  private void FlickerLampToLight()
  {
    lamp.color = new Color(0.8f, 0, 0, 1f);
  }

  private void FlickerLampToDark()
  {
    lamp.color = Color.red;
  }
}
