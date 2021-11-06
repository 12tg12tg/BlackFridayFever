using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    FindMoney,
    FindItem,
    GoTruck,
    Stuned,
    GoCrush,
}

public class AiBehaviour : UnitBehaviour
{
    [Header("(Inspector 수정 불필요)")]
    public GameObject[] moneys;
    public GameObject[] LowItems;
    public GameObject[] MidItems;
    public GameObject[] HighItems;

    public NavMeshAgent agent;
    public Rigidbody rigid;

    private float searchDistance = 8.5f;
    private bool isTarget = false;

    private SpawnObject targetMoney;
    private SpawnObject targetItem;

    public AIState state;
    public AIState prevState;

    private float timer;

    private bool isGameOver;

    public List<CharacterStats> unitList;
    public List<CharacterStats> weakList;
    public CharacterStats tankTarget;
    public bool isCrush;
    public bool isPlayerChase;

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
                case AIState.GoTruck:
                    agent.isStopped = false;
                    setMoveAnimation();
                    break;
                case AIState.Stuned:
                    timer = 0f;
                    break;
                case AIState.GoCrush:
                    isCrush = false;
                    agent.isStopped = false;
                    break;
            }
        }  
    }

    //색배정
    public SkinnedMeshRenderer skin;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.stats.speed;
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();

        var skins = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < skins.Length; i++)
        {
            if (skins[i].tag == "Skin")
            {
                skin = skins[i];
                break;
            }
        }
    }

    public override void Init()
    {
        State = AIState.Idle;
        prevState = AIState.FindMoney;
        transform.position = stats.truck.dokingSpot.position + transform.forward * 2f;
        meshColor = stats.truck.bodyColor;
        skin.material.color = meshColor;

        moneys = Stage.Instance.moneys;
        LowItems = Stage.Instance.LowItems;
        MidItems = Stage.Instance.MidItems;
        HighItems = Stage.Instance.HighItems;

        if(stats.stats.type == UnitType.Ai_Tank)
        {
            InitTankTarget();
        }
    }

    private void Update()
    {
        switch (GameManager.GM.State)
        {
            case GameManager.GameState.Idle:
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                StateUpdate();
                break;
            case GameManager.GameState.End:
                if (!isGameOver && GameManager.GM.winner != stats)
                {
                    isGameOver = true;
                    agent.destination = transform.position;
                    setIdleAnimation();
                }
                break;
        }
        //if(agent.enabled)
        //{
        //    rigid.velocity = Vector3.zero;
        //}
        //애니메이션
        animator.SetInteger("Stack", stats.itemStack);
    }




    private void StateUpdate()
    {
        switch (State)
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
            case AIState.GoTruck:
                GoTruckUpdate();
                break;
            case AIState.Stuned:
                SutnedUpdate();
                break;
            case AIState.GoCrush:
                TankUpdate();
                break;
        }
    }


    public void IdleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            if (stats.stats.type == UnitType.Ai_Tank ||
                stats.stats.type == UnitType.Ai_Tank2)
            {
                var prop = Random.Range(0, 1);
                if(prop < 0.5f)
                {
                    if(TankTarget())
                    {
                        State = AIState.GoCrush;
                        agent.destination = tankTarget.transform.position;
                    }
                    else
                    {
                        State = prevState;
                    }
                }
                else
                {
                    State = prevState;
                }
            }
            else
            {
                State = prevState;
            }
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
        if(targetMoney?.myObject == null || Vector3.Distance(transform.position, agent.destination) < 1f)
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

        //아이템을 주워서 돈을 먹을 수 없게 되었다면?
        if(stats.itemStack != 0)
        {
            State = AIState.GoTruck;
        }
    }

    private void FindItemUpdate()
    {
        /*탈출조건*/
        //목표점수 이상 도달하면, 트럭으로.
        if (stats.stats.type ==UnitType.Ai && stats.score > stats.stats.maximumScore)
        {
            State = AIState.GoTruck;
        }
        else if(stats.stats.type == UnitType.Ai_Money && stats.score + stats.truck.currentScore > GameManager.GM.curStageInfo.goalScore)
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

    public void SutnedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > stats.stats.stunTime)
        {
            stats.isStuned = false;
            //agent.destination = true;
            agent.enabled = true;
            rigid.isKinematic = true;
            DecideState();
        }
    }

    public new void CrushInit()
    {
        base.CrushInit();
        State = AIState.Stuned;
        agent.enabled = false;
        rigid.isKinematic = false;
    }

    public void DecideState()
    {
        if (stats.score > stats.stats.maximumScore)
        {
            State = AIState.GoTruck;
        }
        else if (stats.stats.type == UnitType.Ai && stats.money > stats.stats.maximumMoney)
        {
            State = AIState.FindItem;
        }
        else if(stats.stats.type == UnitType.Ai_Money && stats.money >= 5)
        {
            State = AIState.FindItem;
        }
        else
        {
            State = AIState.FindMoney;
        }
    }

    private void AIStop()
    {
        agent.enabled = false;
    }
    public new void SetWinAnimation()
    {
        base.SetWinAnimation();
        AIStop();
    }
    public new void SetDeafeatAnimation()
    {
        AIStop();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Stumble"))
            base.SetDeafeatAnimation();
    }

    private void InitTankTarget()
    {
        var ais = GameManager.GM.curStageInfo.Ais;
        //unitTargetList = new List<CharacterStats>();
        for (int i = 0; i < ais.Length; i++)
        {
            if(ais[i] != this)
            {
                unitList.Add(ais[i]);
            }
        }
        unitList.Add(GameManager.GM.player.GetComponent<CharacterStats>());
    }

    private bool TankTarget()
    {
        if (stats.stats.type == UnitType.Ai_Tank)
        {
            tankTarget = null;
            weakList.Clear();
            for (int i = 0; i < unitList.Count; i++)
            {
                if (unitList[i].itemStack != 0 && stats.itemStack > unitList[i].itemStack)
                {
                    weakList.Add(unitList[i]);
                }
            }
            if (weakList.Count > 0)
            {
                tankTarget = weakList[Random.Range(0, weakList.Count)];
                if (tankTarget.stats.type == UnitType.Player)
                {
                    isPlayerChase = true;
                }
                else
                {
                    isPlayerChase = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            var playerStat = GameManager.GM.player.GetComponent<CharacterStats>();
            if (playerStat.itemStack != 0 && stats.itemStack > playerStat.itemStack)
            {
                tankTarget = playerStat;
                isPlayerChase = true;
                return true;
            }
            else
            {
                isPlayerChase = false;
                return false;
            }
        }
    }

    private void TankUpdate()
    {
        agent.destination = tankTarget.transform.position;
        if(tankTarget.itemStack >= stats.itemStack)
        {
            State = AIState.FindItem;
        }
        else
        {
            if(isCrush)
            {
                State = AIState.FindItem;
            }
        }
    }
}
