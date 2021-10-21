using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    FindMoney,
    FindItem,
    Standby,
    GoTruck,
    Stuned,
}

public class AiBehaviour : MonoBehaviour
{
    public CreateMoney moneySpots;
    public CreateItem itemSpots;
    private NavMeshAgent agent;
    private Animator animator;
    private CharacterStats stats;
    private float searchDistance = 8.5f;
    private bool isTarget = false;
    private Money targetMoney;
    private ItemScript targetItem;
    private AIState state;
    private AIState prevState;
    public GameObject door;

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
                case AIState.Standby:
                    agent.isStopped = false;
                    break;
                case AIState.GoTruck:
                    agent.isStopped = false;
                    setMoveAnimation();
                    break;
                case AIState.Stuned:
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
            case AIState.Standby:
                StanbyUpdate();
                break;
            case AIState.GoTruck:
                GoTruckUpdate();
                break;
            case AIState.Stuned:

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
            var index =  Random.Range(0, cols.Length);
            targetMoney = cols[index].GetComponent<Money>();
            if (!targetMoney.CanTake)
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
        if(!targetMoney.CanTake)
        {
            State = AIState.Idle;
        }

        //일정 금액이상 모이면, 현재 게임 상태 확인 후 상태변경 출입문앞으로 대기
        if(stats.money > stats.stats.maximumMoney)
        {
            if (GameManager.GM.State != GameManager.GameState.Play)
                State = AIState.Standby;
            else
                State = AIState.FindItem;
        }
    }
    private void StanbyUpdate()
    {
        //문과 가까워지기
        var dis = Vector3.Distance(door.transform.position, transform.position);
        if (dis > 1.5f)
        {
            agent.isStopped = false;
            agent.destination = door.transform.position;
            setMoveAnimation();
        }
        else
        {
            agent.isStopped = true;
            setIdleAnimation();
        }

        //게임시작이면 아이템줍는상태로
        if (GameManager.GM.State == GameManager.GameState.Play)
            State = AIState.FindItem;
    }
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
                targetItem = itemSpots.lowItem[Random.Range(0, itemSpots.lowItem.Length)].GetComponentInChildren<ItemScript>();
            else if (seed < stats.stats.lowValueProp + stats.stats.midValueProp)
                targetItem = itemSpots.midItem[Random.Range(0, itemSpots.midItem.Length)].GetComponentInChildren<ItemScript>();
            else
                targetItem = itemSpots.HighItem[Random.Range(0, itemSpots.HighItem.Length)].GetComponentInChildren<ItemScript>();

            if (!targetItem.CanTake || stats.money < targetItem.info.price)
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
        if (!targetItem.CanTake)
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

    private void AiDance()
    {

    }
}
