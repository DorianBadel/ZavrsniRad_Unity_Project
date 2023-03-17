using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

//TODO make AI stop at a distance from mine
//TODO make effects point to mine
public class RTSBots : MonoBehaviour
{

  [Header("GeneralAI")]
  public Vector3 target;
  public UnityEngine.AI.NavMeshAgent agent;
  public GameObject[] deposits;
  public GameObject[] ores;

  [Header("Stats")]
  public int currentLoad = 0;
  public int maxLoad = 3;
  public float minDist = 3f;

  public StateMachine stateMachine;

  public bool wantsToMine = false;
  private bool digging = false;
  private bool dropping = false;


  void Awake()
  {
    stateMachine = new StateMachine();
  }

  void Start()
  {
    target = transform.position;
    deposits = GameObject.FindGameObjectsWithTag("Deposit");
    ores = GameObject.FindGameObjectsWithTag("Ore");
  }

  void Update()
  {
    if (stateMachine.currentState == StatsAndEvents.States.Mining)
    {
      if (Vector3.Distance(this.transform.position, ores[0].transform.position) < minDist)
      {
        if (currentLoad < maxLoad)
        {
          Mine();
        }
        else goDeposit();
      }
    }


    if (stateMachine.currentState == StatsAndEvents.States.Depositing)
    {
      if (Vector3.Distance(this.transform.position,
      deposits[0].transform.position) < minDist)
      {
        if (currentLoad > 0)
        {
          Deposit();
        }
        else if (wantsToMine) goMine();
        else HideEffects();
      }
    }

    if (stateMachine.currentState == StatsAndEvents.States.Moving || stateMachine.currentState == StatsAndEvents.States.Idle)
    {
      HideEffects();
    }

    agent.SetDestination(target);

  }

  private void SetGoal(Vector3 goal)
  {
    target = goal;
  }

  private void Mine()
  {
    Debug.Log("Im Mining");
    if (!digging) StartCoroutine(Dig());
  }

  private void Deposit()
  {
    Debug.Log("Im Depositing");
    if (!dropping) StartCoroutine(Drop());
  }

  private void goDeposit()
  {
    wantsToMine = true;
    HideEffects();
    stateMachine.makeTransition(StatsAndEvents.Event.CommandToDeposit);
    target = deposits[0].transform.position;
  }

  private void goMine()
  {
    HideEffects();
    stateMachine.makeTransition(StatsAndEvents.Event.CommandToMine);
    target = ores[0].transform.position;
  }


  //Functions for issued commands
  public void MoveHere(Vector3 goal)
  {
    SetGoal(goal);
    stateMachine.makeTransition(StatsAndEvents.Event.CommandToMove);
    wantsToMine = false;
  }

  public void MineHere(Vector3 goal)
  {
    if (currentLoad < maxLoad)
    {
      stateMachine.makeTransition(StatsAndEvents.Event.CommandToMine);
      SetGoal(goal);
    }
    else
    {
      Debug.Log("Storage full");
    }
  }

  public void DepositHere(Vector3 goal)
  {
    if (currentLoad > 0)
    {
      stateMachine.makeTransition(StatsAndEvents.Event.CommandToDeposit);
      wantsToMine = false;
      SetGoal(goal);
    }
    else
    {
      Debug.Log("Nothing to deposit");
    }
  }

  //EFFECT CONTROLS
  private void HideEffects()
  {
    this.transform.Find("Depositing_effect").gameObject.SetActive(false);
    this.transform.Find("Mining_effect").gameObject.SetActive(false);
  }
  private void ShowMining()
  {
    this.transform.Find("Mining_effect").gameObject.SetActive(true);
  }
  private void ShowDepositing()
  {
    this.transform.Find("Depositing_effect").gameObject.SetActive(true);
  }

  //Delay for mining and depositing

  IEnumerator Dig()
  {
    digging = true;
    ShowMining();
    yield return new WaitForSeconds(1);
    if (currentLoad < maxLoad)
      currentLoad++;
    digging = false;
  }

  IEnumerator Drop()
  {
    dropping = true;
    ShowDepositing();
    yield return new WaitForSeconds(1);
    if (currentLoad > 0)
      currentLoad--;
    dropping = false;
  }
}
