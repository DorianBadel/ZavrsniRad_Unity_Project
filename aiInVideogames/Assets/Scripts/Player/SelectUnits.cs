using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnits : MonoBehaviour
{
  public static SelectUnits Instance;

  public Camera Cam;
  public Vector3 MousePosition;
  public List<GameObject> SelectedUnits = new List<GameObject>();

  private RaycastHit hit;

  private MiniGameController miniGameController;


  void Start()
  {
    Instance = this;
  }

  public void HandleUnitSelection()
  {
    MousePosition = Input.mousePosition;

    Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit))
    {
      LayerMask layerHit = hit.transform.gameObject.layer;

      switch (layerHit.value)
      {
        case 6: //units
          SelectUnit(hit.transform.gameObject, Input.GetKey(KeyCode.LeftShift));
          break;
        default:
          DeselectUnits();
          break;
      }
    }
  }

  public void HandleUnitCommand()
  {
    MousePosition = Input.mousePosition;

    Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit))
    {
      LayerMask layerHit = hit.transform.gameObject.layer;

      Debug.Log(layerHit.value);

      switch (layerHit.value)
      {
        case 7: //Deposits
          UnitsDeposit(hit.point);
          break;
        case 8: //Ore
          UnitsStartMining(hit.point);
          break;
        default:
          MoveUnits(hit.point);
          break;
      }
    }
  }

  private void MoveUnits(Vector3 goal)
  {
    foreach (GameObject unit in SelectedUnits)
    {
      unit.GetComponent<RTSBots>().MoveHere(goal);
    }
  }

  private void UnitsStartMining(Vector3 goal)
  {
    foreach (GameObject unit in SelectedUnits)
    {
      unit.GetComponent<RTSBots>().MineHere(goal);
    }
  }

  private void UnitsDeposit(Vector3 goal)
  {
    foreach (GameObject unit in SelectedUnits)
    {
      unit.GetComponent<RTSBots>().DepositHere(goal);
    }
  }

  private void SelectUnit(GameObject unit, bool multiSelect = false)
  {
    if (!multiSelect) DeselectUnits();

    SelectedUnits.Add(unit);
    unit.transform.Find("Marker").gameObject.SetActive(true);
  }

  private void DeselectUnits()
  {
    foreach (GameObject unit in SelectedUnits)
    {
      unit.transform.Find("Marker").gameObject.SetActive(false);
    }
    SelectedUnits.Clear();
  }

}
