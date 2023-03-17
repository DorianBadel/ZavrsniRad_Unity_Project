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

  void Start()
  {
    Instance = this;
  }

  void Update()
  {
    HandleUnitSelection();
  }

  private void HandleUnitSelection()
  {
    if (Input.GetMouseButtonDown(0))
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
            //selecting = true;
            DeselectUnits();
            break;
        }
      }
    }

    if (Input.GetMouseButtonDown(1))
    {
      MousePosition = Input.mousePosition;

      Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit))
      {
        LayerMask layerHit = hit.transform.gameObject.layer;

        switch (layerHit.value)
        {
          case 8:
            UnitsStartMining(hit.point);
            break;
          case 7:
            UnitsDeposit(hit.point);
            break;
          default:
            MoveUnits(hit.point);
            break;
        }

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
