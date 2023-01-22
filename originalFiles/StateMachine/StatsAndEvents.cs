using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{

  public class StatsAndEvents : MonoBehaviour
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
      CommandToDeposit
    }
  }

}
