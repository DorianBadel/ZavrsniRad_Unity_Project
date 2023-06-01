using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockStats : MonoBehaviour
{
  public float maxAmount = 0f;
  public float initialAmount = 0f;
  public Transform stockVisual;
  public Transform animationPosition;
  public float currentAmount;

  private float initialScaleZ;

  void Start()
  {
    currentAmount = initialAmount;
    initialScaleZ = stockVisual.localScale.z;
    UpdateVisual();
  }

  private void UpdateVisual()
  {
    float scaleZ = currentAmount / maxAmount * initialScaleZ;
    stockVisual.localScale = new Vector3(stockVisual.localScale.x, stockVisual.localScale.y, scaleZ);
  }

  public void Increment(float amount = 1f)
  {
    currentAmount += amount;
    UpdateVisual();
  }

  public void Decrement(float amount = 1f)
  {
    currentAmount -= amount;
    UpdateVisual();
  }

  public bool CheckIfHasSpace()
  {
    if (currentAmount < maxAmount) return true;
    return false;
  }

  public bool CheckIfNotEmpty()
  {
    if (currentAmount > 0) return true;
    return false;
  }

  public void Reset()
  {
    currentAmount = initialAmount;
    UpdateVisual();
  }
}
