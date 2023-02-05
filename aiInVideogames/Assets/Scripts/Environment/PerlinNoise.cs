using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
  public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
  {

    float[,] noiseMap = new float[width, height];


    System.Random rand = new System.Random(seed);
    Vector2[] octaveOffsets = new Vector2[octaves];

    for (int i = 0; i < octaves; i++)
    {
      float offsetX = rand.Next(-100000, 100000) + offset.x;
      float offsetY = rand.Next(-100000, 100000) + offset.y;
      octaveOffsets[i] = new Vector2(offsetX, offsetY);

    }

    if (scale <= 0) scale = 0.0001f;

    float halfWidth = width / 2f;
    float halfHeight = height / 2f;

    float maxNoiseHeight = float.MinValue;
    float minNoiseHeight = float.MaxValue;

    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++)
        {
          float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
          float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

          float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; //makes it negative smh
          noiseHeight += perlinValue * amplitude;

          amplitude *= persistance; //decreases 0-1
          frequency *= lacunarity; //increases
        }

        //Normalizing
        if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
        else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;


        noiseMap[x, y] = noiseHeight;
      }
    }

    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //between 0 and 1
      }
    }
    return noiseMap;
  }
}
