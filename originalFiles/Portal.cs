using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject gameMaster;
    private Portal_manager portalManager;
    [Header("Requirements")]
    public Transform portalDestination;
    public bool isPortalForNavMesh = false;
    public bool requireKey = true;

    void Start(){
      gameMaster = GameObject.FindGameObjectWithTag("GameController");
      portalManager = gameMaster.GetComponent<Portal_manager>();

    }

    public void OnTriggerEnter(){
      if(isPortalForNavMesh){
        gameMaster.GetComponent<Camera_controller>().TopDownNavMeshCamera();
      }else{
        if(!requireKey) portalManager.teleportTo(portalDestination,true);
        else portalManager.teleportTo(portalDestination);
      }

    }
}
