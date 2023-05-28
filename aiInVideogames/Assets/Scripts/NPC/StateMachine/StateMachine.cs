using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

  public class StateMachine
  {
    public StatesAndEvents.States currentState { get; set; }

    public StateMachine()
    {
      currentState = StatesAndEvents.States.Idle;
      Debug.Log("initiated");
    }

    public void makeTransition(StatesAndEvents.Event trigger)
    {
      switch (trigger)
      {
        case StatesAndEvents.Event.CommandToMine:
          currentState = StatesAndEvents.States.Mining;
          break;
        case StatesAndEvents.Event.CommandToDeposit:
          currentState = StatesAndEvents.States.Depositing;
          break;
        case StatesAndEvents.Event.CommandToMove:
          currentState = StatesAndEvents.States.Moving;
          break;
        case StatesAndEvents.Event.ReturnToIdle:
          currentState = StatesAndEvents.States.Idle;
          break;
        case StatesAndEvents.Event.InventoryFull:
          currentState = StatesAndEvents.States.Depositing;
          break;
        case StatesAndEvents.Event.InventoryEmpty:
          currentState = StatesAndEvents.States.Mining;
          break;
      }
    }
  }

}
