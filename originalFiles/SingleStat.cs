using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SingleStat
{
    [SerializeField]
    private float initialValue;

    public float GetValue(){
      return initialValue;
    }
}
