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
    private AIState state;
    private AIState prevState;

    private float timer;
    //private 
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
                    break;
                case AIState.Standby:
                    break;
                case AIState.Stuned:
                    break;
            }
        }  
    }

    void setMoveAnimation()
    {
        if (stats.score > 0)
            animator.SetTrigger("LiftRun");
        else
            animator.SetTrigger("JustRun");
    }

    void setIdleAnimation()
    {
        if (stats.score > 0)
            animator.SetTrigger("Lift");
        else
            animator.SetTrigger("JustIdle");
    }







    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<CharacterStats>();
        State = prevState = AIState.FindMoney;
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

                break;
            case AIState.Standby:

                break;
            case AIState.Stuned:

                break;
        }
        //Debug.Log($"{state}, {prevState}");
    }







    public void IdleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 0.3f)
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
            var index =  Random.Range(0, cols.Length);
            targetMoney = cols[index].GetComponent<Money>();
            if (!targetMoney.CanTake)
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
        if(!targetMoney.CanTake)
        {
            State = AIState.Idle;
        }

        //���� �ݾ��̻� ���̸� ���Թ������� ���
        if(stats.money > stats.stats.maximumMoney)
        {
            /*����*/
            State = AIState.Standby;
        }
    }
    private void Move()
    {
        
    }
}
