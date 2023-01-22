using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AllObstacles
{
    private static readonly AllObstacles world = new AllObstacles();
    private static GameObject[] obstacles;

    static AllObstacles(){
      obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private AllObstacles(){}
    public GameObject[] GetObstacles(){
      return obstacles;
    }

    public static AllObstacles World{
      get {return world;}
    }


}
