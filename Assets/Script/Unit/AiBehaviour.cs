using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    FindMoney,
    FindItem,
    //Standby,
    GoTruck,
    Stuned,
}

public class AiBehaviour : MonoBehaviour
{
    public GameObject[] moneys;
    public GameObject[] LowItems;
    public GameObject[] MidItems;
    public GameObject[] HighItems;
    public NavMeshAgent agent;
    private Animator animator;
    private CharacterStats stats;
    private float searchDistance = 8.5f;
    private bool isTarget = false;
    private SpawnObject targetMoney;
    private SpawnObject targetItem;
    private AIState state;
    private AIState prevState;

    private float timer;

    public AIState State
    {
        get
        {
            return state;
        }
        set
        {
            prevState = state;
            state = value;
            //���� �ʱ�ȭ ���� : agent, �ִϸ��̼�, Ÿ�̸�
            isTarget = false;
            switch (value)
            {
                case AIState.Idle:
                    timer = 0f;
                    isTarget = false;
                    agent.isStopped = true;
                    setIdleAnimation();
                    break;
                case AIState.FindMoney:
                    agent.isStopped = false;
                    setMoveAnimation();
                    break;
                case AIState.FindItem:
                    agent.isStopped = false;
                    setMoveAnimation();
                    break;
                //case AIState.Standby:
                //    agent.isStopped = false;
                //    break;
                case AIState.GoTruck:
                    agent.isStopped = false;
                    setMoveAnimation();
                    break;
                case AIState.Stuned:
                    timer = 0f;
                    break;
            }
        }  
    }

    void setMoveAnimation()
    {
        if (stats.score > 0)
        {
            animator.SetTrigger("LiftRun");
            //Debug.Log($"{stats.score}, LiftRun");
        }
        else
        {
            animator.SetTrigger("JustRun");
            //Debug.Log($"{stats.score}, JustRun");
        }
    }

    void setIdleAnimation()
    {
        if (stats.score > 0)
        {
            animator.SetTrigger("Lift");
            //Debug.Log($"��{stats.score}, Lift");
        }
        else
        {
            animator.SetTrigger("JustIdle");
            //Debug.Log($"��{stats.score}, JustIdle");
        }

    }







    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<CharacterStats>();
        State = prevState = AIState.FindMoney;
        transform.position = stats.truck.dokingSpot.position + transform.forward * 3f;

        moneys = GameObject.FindGameObjectsWithTag("Money");
        LowItems = GameObject.FindGameObjectsWithTag("LowItem");
        MidItems = GameObject.FindGameObjectsWithTag("MidItem");
        HighItems = GameObject.FindGameObjectsWithTag("HighItem");
    }
    private void Update()
    {
        switch (state)
        {
            case AIState.Idle:
                IdleUpdate();
                break;
            case AIState.FindMoney:
                FindMoneyUpdate();
                break;
            case AIState.FindItem:
                FindItemUpdate();
                break;
            //case AIState.Standby:
            //    StanbyUpdate();
            //    break;
            case AIState.GoTruck:
                GoTruckUpdate();
                break;
            case AIState.Stuned:
                SutnedUpdate();
                break;
        }
        //Debug.Log($"{state}, {prevState}");
    }







    public void IdleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            State = prevState;
        }
    }
    public void FindMoneyUpdate()
    {
        if(!isTarget)
        {
            //1. Ÿ�ٰ��� : ���� �������� ������Ʈ�� �սô�.  & Ÿ���� activate �׻� üũ
            var cols = Physics.OverlapSphere(transform.position, searchDistance, LayerMask.GetMask("Money"));

            //������ ���� ���ٸ�
            if(cols.Length == 0)
            {
                var noMoney = GameObject.FindGameObjectWithTag("NoMoney");
                agent.destination = noMoney.transform.position;
                isTarget = true;
                return;
            }

            var index =  Random.Range(0, cols.Length);
            targetMoney = cols[index].GetComponentInParent<SpawnObject>();
            if (targetMoney.myObject == null)
            {
                State = AIState.Idle;
                return;
            }
            //2. ã������ �������� ���.
            agent.destination = cols[index].transform.position;
            isTarget = true;
        }

        /*��� Ȯ��*/
        //3. ��Ȱ��ȭ��� �ٸ� Ÿ�� ����
        if(targetMoney.myObject == null || Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            State = AIState.Idle;
        }

        //���� �ݾ��̻� ���̸�, ������ ã����
        if(stats.money > stats.stats.maximumMoney)
        {
            //if (GameManager.GM.State != GameManager.GameState.Play)
            //    State = AIState.Standby;
            //else
            State = AIState.FindItem;
        }
    }

    //private void StanbyUpdate()
    //{
    //    //���� ���������
    //    var dis = Vector3.Distance(door.transform.position, transform.position);
    //    if (dis > 1.5f)
    //    {
    //        agent.isStopped = false;
    //        agent.destination = door.transform.position;
    //        setMoveAnimation();
    //    }
    //    else
    //    {
    //        agent.isStopped = true;
    //        setIdleAnimation();
    //    }

    //    //���ӽ����̸� �������ݴ»��·�
    //    if (GameManager.GM.State == GameManager.GameState.Play)
    //        State = AIState.FindItem;
    //}

    private void FindItemUpdate()
    {
        /*Ż������*/
        //��ǥ���� �̻� �����ϸ�, Ʈ������.
        if (stats.score > stats.stats.maximumScore)
        {
            State = AIState.GoTruck;
        }

        //���� �����ϸ�, Ʈ������.
        if (stats.money < 5)
            State = AIState.GoTruck;

        /*Ÿ�ٻ��*/
        if (!isTarget)
        {
            //1. Ÿ�ٰ��� : ��� ������ ������ �޾ƿɽô�. & �켱���� ���� & Ÿ���� activate �׻� üũ
            var seed = Random.Range(0f, 1f);
            if (seed < stats.stats.lowValueProp)
                targetItem = LowItems[Random.Range(0, LowItems.Length)].GetComponent<SpawnObject>();
            else if (seed < stats.stats.lowValueProp + stats.stats.midValueProp)
                targetItem = MidItems[Random.Range(0, MidItems.Length)].GetComponent<SpawnObject>();
            else
                targetItem = HighItems[Random.Range(0, HighItems.Length)].GetComponent<SpawnObject>();

            //if(targetItem == null)
            //    Debug.Log($"{seed} / Ÿ���� null�̿���");
            //if (targetItem.GetComponentInChildren<ItemScript>() == null)
            //{
            //    Debug.Log($"{seed} / �����۽�ũ��Ʈ�� �����");
            //    Debug.Log($"{targetItem.tag}");
            //}
            //if (targetItem.GetComponentInChildren<ItemScript>().info == null)
            //    Debug.Log($"{seed} / ������������ �����");

            if (targetItem.myObject == null || stats.money < targetItem.GetComponentInChildren<ItemScript>().info.price)
            {
                State = AIState.Idle;
                return;
            }

            //2. ã������ �������� ���.
            agent.destination = targetItem.transform.position;
            isTarget = true;
        }

        /*��� Ȯ��*/
        //3. ��Ȱ��ȭ��� �ٸ� Ÿ�� ����
        if (targetItem.myObject == null)
        {
            State = AIState.Idle;
        }

        setMoveAnimation();

    }

    private void GoTruckUpdate()
    {
        agent.destination = stats.truck.GetComponent<TruckScript>().dokingSpot.position;
        if(!agent.isStopped && Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            agent.isStopped = true;
        }
    }

    public void CrushInit()
    {
        state = AIState.Stuned;
        agent.enabled = false;
        animator.SetTrigger("Stumble");
    }

    public void SutnedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > stats.stats.stunTime)
        {
            stats.isStuned = false;
            agent.enabled = true;
            DecideState();
        }
    }

    public void DecideState()
    {
        if (stats.score > stats.stats.maximumScore)
        {
            State = AIState.GoTruck;
        }
        else if (stats.money > stats.stats.maximumMoney)
        {
            State = AIState.FindItem;
        }
        else
        {
            State = AIState.FindMoney;
        }
    }
}
