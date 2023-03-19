using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreScript : MonoBehaviour
{
  public float maxAmount = 10f;
  public float currentAmount;
  public Transform ore;

  private float initialScaleY;
  private Vector3 initialPosition;

  private void Start()
  {
    currentAmount = maxAmount;
    initialScaleY = ore.localScale.y;
    initialPosition = ore.localPosition;
  }

  private void UpdateVisual()
  {
    float scaleY = currentAmount / maxAmount * initialScaleY;
    ore.localScale = new Vector3(ore.localScale.x, scaleY, ore.localScale.z);

    float deltaY = (initialScaleY - scaleY) * 0.5f;
    ore.localPosition = new Vector3(initialPosition.x, initialPosition.y - deltaY - 0.25f, initialPosition.z);
  }

  public void Mine()
  {
    if (currentAmount > 0) currentAmount--;
    UpdateVisual();
  }

  public bool hasOre()
  {
    if (currentAmount == 0) return false;
    return true;
  }
}
