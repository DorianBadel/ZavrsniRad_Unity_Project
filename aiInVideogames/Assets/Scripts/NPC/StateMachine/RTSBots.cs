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
  private GameObject deposit, ore;
  private StockStats depositStats, resourceStats, thisStorageStats;

  [Header("Stats")]
  public float minDist = 3f;
  public StateMachine stateMachine;
  public bool wantsToMine = false;
  private bool digging = false;
  private bool dropping = false;
  private Vector3 initialPosition;


  void Awake()
  {
    stateMachine = new StateMachine();
  }

  void Start()
  {
    target = transform.position;
    initialPosition = transform.position;
    deposit = GameObject.FindGameObjectWithTag("Deposit");
    ore = GameObject.FindGameObjectWithTag("Ore");
    depositStats = deposit.GetComponent<StockStats>();
    resourceStats = ore.GetComponent<StockStats>();
    thisStorageStats = this.GetComponent<StockStats>();
  }

  void Update()
  {
    // colorSwap();
    if (!resourceStats.CheckIfNotEmpty()) ore = null; //Doesn't look for ore if no more is left

    if (!depositStats.CheckIfHasSpace())
    {
      GameMaster.Instance.CompleteMiniGame("NavMesh");
    }
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
          if (thisStorageStats.CheckIfHasSpace())
          {
            Mine();
          }
          else if (depositStats.CheckIfHasSpace())
          {
            goDeposit();
          }
        }
        break;

      case StatesAndEvents.States.Depositing:
        if (!depositStats.CheckIfHasSpace())
        {
          stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
          break;
        }

        if (CheckDistanceBetween(deposit, this.gameObject))
        {
          StopMovingIfWithinRange(deposit);
          if (thisStorageStats.CheckIfNotEmpty())
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
        if (CheckDistanceBetween(this.gameObject, target, minDist / 2)) stateMachine.makeTransition(StatesAndEvents.Event.ReachedDestination);
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
    if (CheckDistanceBetween(goal, this.gameObject))
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
    if (thisStorageStats.CheckIfHasSpace() && ore != null)
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
    if (thisStorageStats.CheckIfNotEmpty() && depositStats.CheckIfHasSpace())
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
    if (thisStorageStats.CheckIfHasSpace())
    {
      if (ore != null)
      {
        resourceStats.Decrement();
        thisStorageStats.Increment();
      }
    }
    digging = false;
  }

  IEnumerator Drop()
  {
    dropping = true;
    yield return new WaitForSeconds(1);
    if (thisStorageStats.CheckIfNotEmpty())
    {
      if (depositStats.CheckIfHasSpace())
      {
        depositStats.Increment();
        thisStorageStats.Decrement();
      }
    }
    dropping = false;
  }

  public void Reset()
  {
    stateMachine.makeTransition(StatesAndEvents.Event.CommandToMove);
    target = initialPosition;
    wantsToMine = false;
    depositStats.Reset();
    thisStorageStats.Reset();
    resourceStats.Reset();

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
