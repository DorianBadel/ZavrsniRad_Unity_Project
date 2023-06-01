using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
//TODO make effects point to mine
public class RTSBots : MonoBehaviour
{

  [Header("GeneralAI")]
  public Vector3 target;
  public UnityEngine.AI.NavMeshAgent agent;
  private GameObject deposit;
  private DepositScript depositStats;
  private OreScript oreStats;
  private GameObject ore;

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
    deposit = GameObject.FindGameObjectWithTag("Deposit");
    ore = GameObject.FindGameObjectWithTag("Ore");
    depositStats = deposit.GetComponent<DepositScript>();
    oreStats = ore.GetComponent<OreScript>();
  }

  void Update()
  {
    // colorSwap();
    if (!oreStats.hasOre()) ore = null; //Doesn't look for ore if no more is left
    switch (stateMachine.currentState)
    {
      case StatesAndEvents.States.Mining:
        if (ore == null)
        {
          stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
          break;
        }

        if (CheckDistanceBetween(ore, this.gameObject))
        {
          StopMovingIfWithinRange(ore);
          if (currentLoad < maxLoad)
          {
            Mine();
          }
          else if (depositStats.hasRoom())
          {
            goDeposit();
          }
        }
        break;

      case StatesAndEvents.States.Depositing:
        if (!depositStats.hasRoom())
        {
          stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
          break;
        }

        if (CheckDistanceBetween(deposit, this.gameObject))
        {
          StopMovingIfWithinRange(deposit);
          if (currentLoad > 0)
          {
            Deposit();
          }
          else if (wantsToMine && ore != null)
          {
            goMine();
          }
          else
          {
            stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
          }
        }
        break;

      case StatesAndEvents.States.Moving:
        if (CheckDistanceBetween(this.gameObject, target)) stateMachine.makeTransition(StatesAndEvents.Event.ReachedDestination);
        break;
      case StatesAndEvents.States.Idle:
        { target = this.transform.position; }
        break;

      default:
        stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
        break;
    }
    agent.SetDestination(target);
  }

  void StopMovingIfWithinRange(GameObject goal)
  {
    if (CheckDistanceBetween(goal, this.gameObject, minDist / 2))
    {
      target = this.transform.position;
    }
  }

  bool CheckDistanceBetween(GameObject obj, GameObject targetPosition, float minimalDistance = 0.0f)
  {
    if (minimalDistance == 0.0f) minimalDistance = minDist;
    return Vector3.Distance(obj.transform.position, targetPosition.transform.position) < minimalDistance;
  }
  bool CheckDistanceBetween(GameObject obj, Vector3 targetPosition, float minimalDistance = 0.0f)
  {
    if (minimalDistance == 0.0f) minimalDistance = minDist;
    return Vector3.Distance(obj.transform.position, targetPosition) < minimalDistance;
  }

  private void Mine()
  {
    if (!digging) StartCoroutine(Dig());
  }

  private void Deposit()
  {
    if (!dropping) StartCoroutine(Drop());
  }

  private void goDeposit()
  {
    wantsToMine = true;
    stateMachine.makeTransition(StatesAndEvents.Event.InventoryFull);
    target = deposit.transform.position;
  }

  private void goMine()
  {
    stateMachine.makeTransition(StatesAndEvents.Event.InventoryEmpty);
    target = ore.transform.position;
  }


  //Functions for issued commands
  public void MoveHere(Vector3 goal)
  {
    target = goal;
    stateMachine.makeTransition(StatesAndEvents.Event.CommandToMove);
    wantsToMine = false;
  }

  public void MineHere(Vector3 goal)
  {
    if (currentLoad < maxLoad && ore != null)
    {
      stateMachine.makeTransition(StatesAndEvents.Event.CommandToMine);
      target = goal;
    }
    else
    {
      Debug.Log("Storage full");
    }
  }

  public void DepositHere(Vector3 goal)
  {
    if (currentLoad > 0 && depositStats.hasRoom())
    {
      stateMachine.makeTransition(StatesAndEvents.Event.CommandToDeposit);
      wantsToMine = false;
      target = goal;
    }
    else
    {
      Debug.Log("Nothing to deposit");
    }
  }


  //Delay for mining and depositing
  IEnumerator Dig()
  {
    digging = true;
    yield return new WaitForSeconds(1);
    if (currentLoad < maxLoad)
    {
      if (ore != null)
      {
        oreStats.Mine();
        currentLoad++;
      }
    }
    digging = false;
  }

  IEnumerator Drop()
  {
    dropping = true;
    yield return new WaitForSeconds(1);
    if (currentLoad > 0)
    {
      if (depositStats.hasRoom())
      {
        depositStats.Deposit();
        currentLoad--;
      }
    }
    dropping = false;
  }

  // private void colorSwap()
  // {
  //   Material renderer = GetComponent<Renderer>().material;
  //   switch (stateMachine.currentState)
  //   {
  //     case StatesAndEvents.States.Mining:
  //       Material pastelRed = new Material(Shader.Find("Standard"));
  //       pastelRed.color = new Color(1f, 0.6f, 0.6f);
  //       renderer.color = pastelRed.color;
  //       break;

  //     case StatesAndEvents.States.Depositing:
  //       Material pastelGreen = new Material(Shader.Find("Standard"));
  //       pastelGreen.color = new Color(0.6f, 1f, 0.6f);
  //       renderer.color = pastelGreen.color;
  //       break;
  //     case StatesAndEvents.States.Idle:
  //       Material pastelBlue = new Material(Shader.Find("Standard"));
  //       pastelBlue.color = new Color(0.6f, 0.6f, 1f);
  //       renderer.color = pastelBlue.color;
  //       break;
  //     case StatesAndEvents.States.Moving:
  //       Material pastelYellow = new Material(Shader.Find("Standard"));
  //       pastelYellow.color = new Color(1f, 1f, 0.6f);
  //       renderer.color = pastelYellow.color;
  //       break;
  //   }
  // }
}
