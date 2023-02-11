using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

  public enum DrawMode
  {
    NoiseMap,
    ColourMap,
    MeshMap,
    FalloffMap
  }

  public DrawMode drawMode;
  public int width;
  public int height;
  public float noiseScale;

  public int octaves;
  [Range(0, 1)]
  public float persistance;
  public float lacunarity;

  public int seed;
  public Vector2 offset;

  public bool useFalloff;

  public float meshHeightMultiplier;
  public AnimationCurve meshHeightCurve;

  public bool autoUpdate;
  public TerrainType[] regions;

  float[,] falloffMap;

  void Awake()
  {
    falloffMap = FalloffGenerator.GenerateFalloffMap(width);
  }

  public void GenerateMap()
  {
    float[,] noiseMap = PerlinNoise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

    Color[] colourMap = new Color[width * height];
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        if (useFalloff)
        {
          noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
        }
        float currentHeight = noiseMap[x, y];
        for (int i = 0; i < regions.Length; i++)
        {
          if (currentHeight <= regions[i].height)
          {
            colourMap[y * width + x] = regions[i].colour;
            break;

          }

        }

      }
    }

    MapDisplay display = FindObjectOfType<MapDisplay>();
    if (drawMode == DrawMode.NoiseMap) display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
    else if (drawMode == DrawMode.ColourMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, width, height));

    }
    else if (drawMode == DrawMode.MeshMap)
    {
      display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap(colourMap, width, height));
    }
    else if (drawMode == DrawMode.FalloffMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromHeightMap(falloffMap));
    }

  }

  void OnValidate()
  {
    if (width < 1) width = 1;
    if (height < 1) height = 1;

    if (lacunarity < 1) lacunarity = 1;
    if (octaves < 0) octaves = 0;
    if (octaves > 100) octaves = 100;

    falloffMap = FalloffGenerator.GenerateFalloffMap(width);
  }
}
[System.Serializable]
public struct TerrainType
{
  public string terrainName;
  public float height;
  public Color colour;
}