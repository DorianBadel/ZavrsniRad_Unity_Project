using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_controller : MonoBehaviour
{
    [Header("Adjustable variables")]
    public float duration = 5f;
    void Start()
    {
      Destroy(gameObject, duration);
    }
}
