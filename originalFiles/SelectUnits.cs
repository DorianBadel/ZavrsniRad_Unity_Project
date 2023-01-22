using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnits : MonoBehaviour
{

    //TODO reformat code
    private RaycastHit hit;
    public static SelectUnits instance;
    public Camera cam;

    private List<GameObject> selectedUnits = new List<GameObject>();

    private Vector3 mousePosition;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
      HandleUnitSelection();
    }

    private void HandleUnitSelection(){
      if(Input.GetMouseButtonDown(0)){
        mousePosition = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit)){
          LayerMask layerHit = hit.transform.gameObject.layer;

          switch(layerHit.value){
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

      if(Input.GetMouseButtonDown(1)){
        mousePosition = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit)){
          LayerMask layerHit = hit.transform.gameObject.layer;

          switch (layerHit.value)
          {
              case 7:
                UnitsStartMining(hit.point);
              break;
              case 8:
                UnitsDeposit(hit.point);
              break;
              default:
                  MoveUnits(hit.point);
              break;
          }

        }

      }
    }

    private void MoveUnits(Vector3 goal){
      foreach(GameObject unit in selectedUnits){
        unit.GetComponent<RTSBots>().MoveHere(goal);
      }
    }

    private void UnitsStartMining(Vector3 goal){
      foreach(GameObject unit in selectedUnits){
        unit.GetComponent<RTSBots>().MineHere(goal);
      }
    }

    private void UnitsDeposit(Vector3 goal){
      foreach(GameObject unit in selectedUnits){
        unit.GetComponent<RTSBots>().DepositHere(goal);
      }
    }

    private void SelectUnit(GameObject unit,bool multiSelect = false){
      if(!multiSelect) DeselectUnits();

      selectedUnits.Add(unit);
      unit.transform.Find("Marker").gameObject.SetActive(true);
    }

    private void DeselectUnits(){
      foreach(GameObject unit in selectedUnits){
        unit.transform.Find("Marker").gameObject.SetActive(false);
      }
      selectedUnits.Clear();
    }

}
