using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownTrigger : MonoBehaviour
{

    public Camera_controller gameMaster;

    void Start(){
      gameMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<Camera_controller>();
      gameMaster.FirstPersonCamera();
    }

    public void OnTriggerEnter(){
      gameMaster.TopDownMazeCamera();
    }

    public void OnTriggerExit(){
      gameMaster.FirstPersonCamera();
    }
}
