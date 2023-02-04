using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
  public int width;
  public int height;
  public float noiseScale;

  public bool autoUpdate;

  public void GenerateMap()
  {
    float[,] noiseMap = PerlinNoise.GenerateNoiseMap(width, height, noiseScale);

    MapDisplay display = FindObjectOfType<MapDisplay>();
    display.DrawNoiseMap(noiseMap);
  }
}
