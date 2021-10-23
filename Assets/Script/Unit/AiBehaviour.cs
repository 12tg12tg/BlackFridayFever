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
            //변수 초기화 영역 : agent, 애니메이션, 타이머
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
            //Debug.Log($"★{stats.score}, Lift");
        }
        else
        {
            animator.SetTrigger("JustIdle");
            //Debug.Log($"★{stats.score}, JustIdle");
        }

    }







    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<CharacterStats>();
        State = prevState = AIState.FindMoney;
        transform.position = stats.truck.dokingSpot.position;

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
            //1. 타겟결정 : 일정 범위내의 오브젝트로 합시다.  & 타겟의 activate 항상 체크
            var cols = Physics.OverlapSphere(transform.position, searchDistance, LayerMask.GetMask("Money"));

            //범위내 돈이 없다면
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
            //2. 찾았으면 목적지로 삼기.
            agent.destination = cols[index].transform.position;
            isTarget = true;
        }

        /*상시 확인*/
        //3. 비활성화라면 다른 타겟 결정
        if(targetMoney.myObject == null || Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            State = AIState.Idle;
        }

        //일정 금액이상 모이면, 아이템 찾으러
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
    //    //문과 가까워지기
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

    //    //게임시작이면 아이템줍는상태로
    //    if (GameManager.GM.State == GameManager.GameState.Play)
    //        State = AIState.FindItem;
    //}

    private void FindItemUpdate()
    {
        /*탈출조건*/
        //목표점수 이상 도달하면, 트럭으로.
        if (stats.score > stats.stats.maximumScore)
        {
            State = AIState.GoTruck;
        }

        //돈이 부족하면, 트럭으로.
        if (stats.money < 5)
            State = AIState.GoTruck;

        /*타겟삼기*/
        if (!isTarget)
        {
            //1. 타겟결정 : 모든 아이템 정보를 받아옵시다. & 우선도를 따라서 & 타겟의 activate 항상 체크
            var seed = Random.Range(0f, 1f);
            if (seed < stats.stats.lowValueProp)
                targetItem = LowItems[Random.Range(0, LowItems.Length)].GetComponent<SpawnObject>();
            else if (seed < stats.stats.lowValueProp + stats.stats.midValueProp)
                targetItem = MidItems[Random.Range(0, MidItems.Length)].GetComponent<SpawnObject>();
            else
                targetItem = HighItems[Random.Range(0, HighItems.Length)].GetComponent<SpawnObject>();

            //if(targetItem == null)
            //    Debug.Log($"{seed} / 타겟이 null이에요");
            //if (targetItem.GetComponentInChildren<ItemScript>() == null)
            //{
            //    Debug.Log($"{seed} / 아이템스크립트가 없어요");
            //    Debug.Log($"{targetItem.tag}");
            //}
            //if (targetItem.GetComponentInChildren<ItemScript>().info == null)
            //    Debug.Log($"{seed} / 아이템인포가 없어요");

            if (targetItem.myObject == null || stats.money < targetItem.GetComponentInChildren<ItemScript>().info.price)
            {
                State = AIState.Idle;
                return;
            }

            //2. 찾았으면 목적지로 삼기.
            agent.destination = targetItem.transform.position;
            isTarget = true;
        }

        /*상시 확인*/
        //3. 비활성화라면 다른 타겟 결정
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
