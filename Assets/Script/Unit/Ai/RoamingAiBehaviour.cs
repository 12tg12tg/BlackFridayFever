using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingAiBehaviour : UnitBehaviour
{
    [Header("(Main에서 사용할 로머들에서만 씁시다.)")]
    public bool isForMain;
    public GameObject[] wayPoints;

    [Header("(Inspector 수정 불필요)")]
    public GameObject[] moneys;
    public GameObject[] LowItems;
    public GameObject[] MidItems;
    public GameObject[] HighItems;

    public NavMeshAgent agent;
    private bool isTarget;
    Transform target;

    public GameObject stickman;
    public AiRagdoll ragdoll;

    private bool isKnockDown;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats.stats.speed;
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("Stack", 0);
        setMoveAnimation();
    }

    public void Init(bool isForMain, GameObject[] wayPoints)
    {
        this.isForMain = isForMain;
        if (!isForMain)
        {
            moneys = Stage.Instance.moneys;
            LowItems = Stage.Instance.LowItems;
            MidItems = Stage.Instance.MidItems;
            HighItems = Stage.Instance.HighItems;
        }
        else
        {
            this.wayPoints = wayPoints;
        }
        setMoveAnimation();
    }

    private void Update()
    {
        if (isForMain)
        {
            StateUpdate();
        }
        else
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
                    setIdleAnimation();
                    agent.destination = transform.position;
                    break;
            }
        }
    }
    private void StateUpdate()
    {
        if (!isTarget)
        {
            if (!isForMain)
            {
                //확률로 일정 지점 배회하기
                var prop = Random.Range(0f, 1f);
                if (prop < 0.3)
                {
                    //저
                    target = LowItems[Random.Range(0, LowItems.Length)].transform;
                }
                else if (prop < 0.6)
                {
                    //중
                    target = MidItems[Random.Range(0, MidItems.Length)].transform;

                }
                else if (prop < 0.9)
                {
                    //고
                    target = HighItems[Random.Range(0, HighItems.Length)].transform;
                }
                else
                {
                    //돈
                    target = moneys[Random.Range(0, moneys.Length)].transform;
                }
            }
            else
            {
                //지정된 장소중 한군데를 목표로 삼기
                target = wayPoints[Random.Range(0, wayPoints.Length)].transform;
            }
            isTarget = true;
            agent.destination = target.transform.position;
        }
        else
        {
            if(Vector3.Distance(transform.position, target.transform.position) < 1.5f)
            {
                isTarget = false;
                
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        var playerController = collision.gameObject.GetComponentInParent<PlayerController>();
        bool isPlayer = playerController != null;
        CharacterStats stats;
        bool isStrong;

        if (isPlayer)
        {
            stats = playerController.stats;
            isStrong = stats.itemStack > 5;
        }
        else
            return;

        if (!isKnockDown && isStrong)
        {
            isKnockDown = true;

            var playerPos = playerController.transform.position;

            var direction = transform.position - playerPos;
            direction = direction.normalized * 400;
            direction.y += 500f;

            ragdoll = GameObjectPool.Instance.GetObject(PoolTag.Ragdoll).GetComponent<AiRagdoll>();
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;
            ragdoll.spine.AddForce(direction, ForceMode.Impulse);

            var colliders = ragdoll.GetComponentsInChildren<Collider>();
            foreach (var item in colliders)
            {
                item.isTrigger = true;
            }

            gameObject.SetActive(false);  //해제
        }
    }
}
