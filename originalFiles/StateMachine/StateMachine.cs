using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

  public class StateMachine
  {
      public StatsAndEvents.States currentState { get; set;}

      public StateMachine(){
        currentState = StatsAndEvents.States.Idle;
        Debug.Log("initiated");
      }

      public void makeTransition(StatsAndEvents.Event trigger){
        switch(trigger)
        {
          case StatsAndEvents.Event.CommandToMine:
            currentState = StatsAndEvents.States.Mining;
          break;
          case StatsAndEvents.Event.CommandToDeposit:
            currentState = StatsAndEvents.States.Depositing;
          break;
          case StatsAndEvents.Event.CommandToMove:
            currentState = StatsAndEvents.States.Moving;
          break;
        }
      }
  }

}
