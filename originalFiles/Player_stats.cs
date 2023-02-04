using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_stats : MonoBehaviour
{
    //TODO make singleton i guess or sth else
    public bool HasKey = false;
    public bool FirstPersonControlls = true;
    public bool IsUnderwater = false;
    public bool IsDetected = false;
    public bool IsDisabled = false;

    public void PickUpKey(){
      HasKey = true;
    }

    public void DropKey(){
      HasKey = false;
    }

    public void SetFirstPersonControlls(bool isFirstPerson){
      FirstPersonControlls = isFirstPerson;
    }

    public void SetDetected(bool hasBeenDetected){
      IsDetected = hasBeenDetected;
      if(IsDetected == true) DropKey();
    }


}
