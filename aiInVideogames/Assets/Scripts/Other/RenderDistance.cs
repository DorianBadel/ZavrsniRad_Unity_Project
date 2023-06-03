using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDistance : MonoBehaviour
{
  public Chunks[] chunks;

  private GameObject player;

  void Awake()
  {
    player = GameObject.FindGameObjectWithTag("Player");
  }

  void FixedUpdate()
  {
    CalculateChunkDistances();
    Chunks[] closestChunks = CalculateClosestChunks();
    ActivateChunks(closestChunks);
  }


  private void CalculateChunkDistances()
  {
    foreach (Chunks chunk in chunks)
    {
      chunk.distance = Vector3.Distance(player.transform.position, chunk.transform.position);
    }
  }

  private Chunks[] CalculateClosestChunks()
  {
    Chunks[] closestChunks = new Chunks[Mathf.Min(chunks.Length, 5)];

    Array.Sort(chunks, (a, b) => a.distance.CompareTo(b.distance));

    // Create a new array with the first 5 objects (smallest values)
    Array.Copy(chunks, closestChunks, closestChunks.Length);

    return closestChunks;
  }

  private void ActivateChunks(Chunks[] closestChunks)
  {
    foreach (Chunks chunk in closestChunks)
    {
      chunk.Activate();
    }
    for (int i = closestChunks.Length; i < chunks.Length; i++)
    {
      chunks[i].Deactivate();
    }
  }


}
