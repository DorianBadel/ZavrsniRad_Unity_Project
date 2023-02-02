using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FindAllObstacles
{
    private static GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    private static readonly FindAllObstacles instance = new FindAllObstacles();

    private FindAllObstacles(){}

    public static FindAllObstacles Instance
    {
        get { return instance; }
    }

    public GameObject[] GetObstacles()
    {
        return obstacles;
    }
}
