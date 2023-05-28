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
  public GameObject deposit;
  private DepositScript depositStats;
  private OreScript oreStats;
  public GameObject ore;

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
    colorSwap();
    if (!oreStats.hasOre()) ore = null;
    switch (stateMachine.currentState)
    {
      case StatesAndEvents.States.Mining:
        if (ore == null)
        {
          stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
          break;
        }
        if (GetDistanceToClosestVertex(ore, this.transform.position) < minDist)
        {
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
        if (GetDistanceToClosestVertex(deposit, this.transform.position) < minDist)
        {
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
            HideEffects();
          }
        }
        break;

      case StatesAndEvents.States.Moving:
        HideEffects();
        break;
      case StatesAndEvents.States.Idle:
        { target = this.transform.position; HideEffects(); }
        break;

      default:
        stateMachine.makeTransition(StatesAndEvents.Event.ReturnToIdle);
        break;
    }

    agent.SetDestination(target);
  }

  float GetDistanceToClosestVertex(GameObject obj, Vector3 targetPosition)
  {
    Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
    Vector3[] vertices = mesh.vertices;
    float minDistance = float.MaxValue;
    foreach (Vector3 vertex in vertices)
    {
      Vector3 worldVertex = obj.transform.TransformPoint(vertex);
      float distance = Vector3.Distance(worldVertex, targetPosition);
      if (distance < minDistance)
      {
        minDistance = distance;
      }
    }
    return minDistance;
  }


  private void SetGoal(Vector3 goal)
  {
    target = goal;
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
    HideEffects();
    stateMachine.makeTransition(StatesAndEvents.Event.InventoryFull);
    target = deposit.transform.position;
  }

  private void goMine()
  {
    HideEffects();
    stateMachine.makeTransition(StatesAndEvents.Event.InventoryEmpty);
    target = ore.transform.position;
  }


  //Functions for issued commands
  public void MoveHere(Vector3 goal)
  {
    SetGoal(goal);
    stateMachine.makeTransition(StatesAndEvents.Event.CommandToMove);
    wantsToMine = false;
  }

  public void MineHere(Vector3 goal)
  {
    if (currentLoad < maxLoad && ore != null)
    {
      stateMachine.makeTransition(StatesAndEvents.Event.CommandToMine);
      SetGoal(goal);
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
    ShowDepositing();
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

  private void colorSwap()
  {
    Material renderer = GetComponent<Renderer>().material;
    switch (stateMachine.currentState)
    {
      case StatesAndEvents.States.Mining:
        Material pastelRed = new Material(Shader.Find("Standard"));
        pastelRed.color = new Color(1f, 0.6f, 0.6f);
        renderer.color = pastelRed.color;
        break;

      case StatesAndEvents.States.Depositing:
        Material pastelGreen = new Material(Shader.Find("Standard"));
        pastelGreen.color = new Color(0.6f, 1f, 0.6f);
        renderer.color = pastelGreen.color;
        break;
      case StatesAndEvents.States.Idle:
        Material pastelBlue = new Material(Shader.Find("Standard"));
        pastelBlue.color = new Color(0.6f, 0.6f, 1f);
        renderer.color = pastelBlue.color;
        break;
      case StatesAndEvents.States.Moving:
        Material pastelYellow = new Material(Shader.Find("Standard"));
        pastelYellow.color = new Color(1f, 1f, 0.6f);
        renderer.color = pastelYellow.color;
        break;
    }
  }
}
