using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositScript : MonoBehaviour
{
  public float maxAmount = 25f;
  public float currentAmount = 0f;
  public Transform[] stores;

  private float initialScaleY;
  private Vector3 initialPosition;

  private void Start()
  {
    initialScaleY = stores[0].localScale.y;
  }

  private void UpdateVisual()
  {
    int i = Mathf.FloorToInt((float)(currentAmount) / (float)3);
    while (i >= stores.Length) i -= stores.Length;

    float scaleY = 1f;
    stores[i].localScale = new Vector3(stores[i].localScale.x, stores[i].localScale.y + scaleY, stores[i].localScale.z);

    float deltaY = scaleY * 0.5f;
    stores[i].localPosition = new Vector3(stores[i].localPosition.x, stores[i].localPosition.y + deltaY + 0.25f, stores[i].localPosition.z);
  }

  public bool hasRoom()
  {
    if (currentAmount < maxAmount) return true;
    return false;
  }

  public void Deposit()
  {
    currentAmount++;
    UpdateVisual();
  }

}
