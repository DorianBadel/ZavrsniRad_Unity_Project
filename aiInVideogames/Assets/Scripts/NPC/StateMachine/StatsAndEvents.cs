using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

  public class StatesAndEvents : MonoBehaviour
  {
    public enum States
    {
      Idle,
      Moving,
      Mining,
      Depositing
    }

    public enum Event
    {
      CommandToMine,
      CommandToMove,
      CommandToDeposit,
      ReturnToIdle,
      InventoryFull,
      InventoryEmpty
    }
  }

}
