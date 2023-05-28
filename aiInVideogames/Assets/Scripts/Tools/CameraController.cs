using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Camera[] cameras;
  private int activeCameraIndex;

  void Start()
  {
    activeCameraIndex = 0;

    for (int i = 0; i < cameras.Length; i++)
    {
      cameras[i].gameObject.SetActive(false);
    }

    if (cameras.Length > 0)
    {
      cameras[activeCameraIndex].gameObject.SetActive(true);
    }
  }

  public void SetActiveCamera(string cameraName)
  {
    for (int i = 0; i < cameras.Length; i++)
    {
      cameras[i].gameObject.SetActive(false);
      if (cameras[i].name == cameraName)
      {
        activeCameraIndex = i;
      }
    }

    cameras[activeCameraIndex].gameObject.SetActive(true);
  }

  public string GetActiveCameraName()
  {
    return cameras[activeCameraIndex].name;
  }

}
