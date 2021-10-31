using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoamingAiBehaviour : UnitBehaviour
{
    [Header("(Inspector 수정 불필요)")]
    public GameObject[] moneys;
    public GameObject[] LowItems;
    public GameObject[] MidItems;
    public GameObject[] HighItems;

    public NavMeshAgent agent;
    private bool isTarget;
    SpawnObject target;

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

    private void Start()
    {
        moneys = Stage.Instance.moneys;
        LowItems = Stage.Instance.LowItems;
        MidItems = Stage.Instance.MidItems;
        HighItems = Stage.Instance.HighItems;
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
                break;
        }
    }
    private void StateUpdate()
    {
        if (!isTarget)
        {
            //확률로 일정 지점 배회하기
            var prop = Random.Range(0f, 1f);
            if (prop < 0.3)
            {
                //저
                target = LowItems[Random.Range(0, LowItems.Length)].GetComponent<SpawnObject>();
            }
            else if (prop < 0.6)
            {
                //중
                target = MidItems[Random.Range(0, MidItems.Length)].GetComponent<SpawnObject>();

            }
            else if (prop < 0.9)
            {
                //고
                target = HighItems[Random.Range(0, HighItems.Length)].GetComponent<SpawnObject>();
            }
            else
            {
                //돈
                target = moneys[Random.Range(0, moneys.Length)].GetComponent<SpawnObject>();
            }
            isTarget = true;
            agent.destination = target.transform.position;
        }
        else
        {
            if(Vector3.Distance(transform.position, target.transform.position) < 1f)
            {
                isTarget = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!isKnockDown && collision.gameObject.tag == "Player" &&
            collision.gameObject.GetComponent<CharacterStats>().itemStack > 5)
        {
            isKnockDown = true;

            var direction = transform.position - collision.transform.position;
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
