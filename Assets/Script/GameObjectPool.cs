using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PoolTag
{
    RoamingAI,
    Ragdoll,
    Money,
    Box_Low,
    Box_Mid,
    Box_High,
    Item_Low_Donut,
    Item_Low_Hamburger,
    Item_Mid_Dog,
    Item_Mid_Cat,
    Item_High_Ring,
    Item_High_Necklace,

}

public class GameObjectPool : MonoBehaviour
{
    private int poolCount = 50;
    public static GameObjectPool Instance;
    public GameObject roamingAI;
    public GameObject ragdollPrefab;
    public GameObject moneyPrefab;
    public GameObject lowBoxPrefab;
    public GameObject midBoxPrefab;
    public GameObject highBoxPrefab;
    public GameObject item_Low_Donut_Prefab;
    public GameObject item_Low_Hamburger_Prefab;
    public GameObject item_Mid_Dog_Prefab;
    public GameObject item_Mid_Cat_Prefab;
    public GameObject item_High_Ring_Prefab;
    public GameObject item_High_Necklace_Prefab;

    //Low, Mid, High Range
    public int LowMin;
    public int LowMax;
    public int MidMin;
    public int MidMax;
    public int HighMin;
    public int HighMax;

    //Dictionary肺 钱 包府
    public Dictionary<PoolTag, Queue<GameObject>> pool = new Dictionary<PoolTag, Queue<GameObject>>();
    public Dictionary<PoolTag, GameObject> prefabs = new Dictionary<PoolTag, GameObject>();
    private void Awake()
    {
        Instance = this;

        //橇府崎
        prefabs.Add(PoolTag.RoamingAI, roamingAI);
        prefabs.Add(PoolTag.Ragdoll, ragdollPrefab);
        prefabs.Add(PoolTag.Box_Low, lowBoxPrefab);
        prefabs.Add(PoolTag.Box_Mid, midBoxPrefab);
        prefabs.Add(PoolTag.Box_High, highBoxPrefab);
        prefabs.Add(PoolTag.Money, moneyPrefab);
        prefabs.Add(PoolTag.Item_Low_Donut, item_Low_Donut_Prefab);
        prefabs.Add(PoolTag.Item_Low_Hamburger, item_Low_Hamburger_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Dog, item_Mid_Dog_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Cat, item_Mid_Cat_Prefab);
        prefabs.Add(PoolTag.Item_High_Ring, item_High_Ring_Prefab);
        prefabs.Add(PoolTag.Item_High_Necklace, item_High_Necklace_Prefab);

        //For Range
        LowMin = (int)PoolTag.Item_Low_Donut;
        LowMax = (int)PoolTag.Item_Low_Hamburger;
        MidMin = (int)PoolTag.Item_Mid_Dog;
        MidMax = (int)PoolTag.Item_Mid_Cat;
        HighMin = (int)PoolTag.Item_High_Ring;
        HighMax = (int)PoolTag.Item_High_Necklace;

        //钱
        pool.Add(PoolTag.RoamingAI, new Queue<GameObject>());
        pool.Add(PoolTag.Ragdoll, new Queue<GameObject>());
        pool.Add(PoolTag.Box_Low, new Queue<GameObject>());
        pool.Add(PoolTag.Box_Mid, new Queue<GameObject>());
        pool.Add(PoolTag.Box_High, new Queue<GameObject>());
        pool.Add(PoolTag.Money, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Low_Donut, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Low_Hamburger, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Dog, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Cat, new Queue<GameObject>());
        pool.Add(PoolTag.Item_High_Ring, new Queue<GameObject>());
        pool.Add(PoolTag.Item_High_Necklace, new Queue<GameObject>());


        //积己
        foreach (var key in pool.Keys)
        {
            for (int i = 0; i < poolCount; i++)
            {
                pool[key].Enqueue(CreateNewObject(key));
            }
        }
    }
    private GameObject CreateNewObject(PoolTag key)
    {
        var newGo = Instantiate(prefabs[key]);
        newGo.transform.SetParent(transform);
        newGo.SetActive(false);
        return newGo;
    }
    public GameObject GetObject(PoolTag key)
    {
        var queue = pool[key];
        if (queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.transform.SetParent(null);
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
            var agent = obj.GetComponent<NavMeshAgent>();
            if(agent != null)
                agent.enabled = true;
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(key);
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            var agent = newObj.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.enabled = true;
            return newObj;
        }
    }
    public void ReturnObject(PoolTag key, GameObject go)
    {
        var queue = pool[key];
        go.transform.SetParent(transform);
        queue.Enqueue(go);
        var agent = go.GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;
        go.SetActive(false);
    }
}
