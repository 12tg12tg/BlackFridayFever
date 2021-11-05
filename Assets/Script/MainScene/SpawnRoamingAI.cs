using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRoamingAI : MonoBehaviour
{
    public bool isForMain;
    public int currentNum;
    public int maxNum;

    public GameObject[] wayPoints;
    public Transform[] aiSpawnZone;
    public Transform parent;

    public float timer;
    public float delay;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (currentNum >= maxNum) return;
        if (GameManager.GM.State == GameManager.GameState.End) return;
        timer += Time.deltaTime;
        if(timer > delay)
        {
            timer = 0f;
            ++currentNum;
            var go = GameObjectPool.Instance.GetObject(PoolTag.RoamingAI);
            go.transform.SetParent(parent);
            go.transform.position = aiSpawnZone[Random.Range(0, aiSpawnZone.Length)].position;
            go.GetComponent<NavMeshAgent>().enabled = true;
            go.GetComponent<RoamingAiBehaviour>().Init(isForMain, wayPoints);
        }
    }
}
