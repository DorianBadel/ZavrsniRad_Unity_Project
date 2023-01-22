using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FindAllObstacles
{
    private static readonly FindAllObstacles world = new FindAllObstacles();
    private static GameObject[] obstacles;

    static FindAllObstacles(){
      obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    }

    private FindAllObstacles(){}
    public GameObject[] GetObstacles(){
      return obstacles;
    }

    public static FindAllObstacles World{
      get {return world;}
    }


}
