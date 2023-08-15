using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kôd pripada Sebastian-u Lague-u
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
  const int mapChunkSize = 241;

  [Range(0, 6)]
  public int levelOfDetail;
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
    falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
  }

  void Start()
  {
    GenerateMap();
  }

  public void GenerateMap()
  {
    float[,] noiseMap = PerlinNoise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

    Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
    for (int y = 0; y < mapChunkSize; y++)
    {
      for (int x = 0; x < mapChunkSize; x++)
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
            colourMap[y * mapChunkSize + x] = regions[i].colour;
            break;

          }

        }

      }
    }

    MapDisplay display = FindObjectOfType<MapDisplay>();
    if (drawMode == DrawMode.NoiseMap) display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
    else if (drawMode == DrawMode.ColourMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));

    }
    else if (drawMode == DrawMode.MeshMap)
    {
      display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
    }
    else if (drawMode == DrawMode.FalloffMap)
    {
      display.DrawTexture(TextureGenerator.TextureFromHeightMap(falloffMap));
    }

  }

  void OnValidate()
  {
    if (lacunarity < 1) lacunarity = 1;
    if (octaves < 0) octaves = 0;
    if (octaves > 100) octaves = 100;

    falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
  }
}
[System.Serializable]
public struct TerrainType
{
  public string terrainName;
  public float height;
  public Color colour;
}
