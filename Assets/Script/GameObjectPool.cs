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
    Item_Low_Burrito,
    Item_Low_Pizza,
    Item_Low_Shoe,
    Item_Mid_Dog,
    Item_Mid_Cat,
    Item_Mid_Album,
    Item_Mid_Banana,
    Item_Mid_Cake,
    Item_Mid_Pineapple,
    Item_Mid_Shoe,
    Item_High_Ring,
    Item_High_Necklace,
    Item_High_Guitar,
    Item_High_Monitor,
    Item_High_Sushi,

}

public class GameObjectPool : Singleton<GameObjectPool>
{
    private int poolCount = 40;

    //public static GameObjectPool Instance;

    public GameObject roamingAI;
    public GameObject ragdollPrefab;
    public GameObject moneyPrefab;
    public GameObject lowBoxPrefab;
    public GameObject midBoxPrefab;
    public GameObject highBoxPrefab;
    public GameObject item_Low_Donut_Prefab;
    public GameObject item_Low_Hamburger_Prefab;
    public GameObject item_Low_Burrito_Prefab;
    public GameObject item_Low_Pizza_Prefab;
    public GameObject item_Low_Shoe_Prefab;
    public GameObject item_Mid_Dog_Prefab;
    public GameObject item_Mid_Cat_Prefab;
    public GameObject item_Mid_Album_Prefab;
    public GameObject item_Mid_Banana_Prefab;
    public GameObject item_Mid_Cake_Prefab;
    public GameObject item_Mid_Pineapple_Prefab;
    public GameObject item_Mid_Shoe_Prefab;
    public GameObject item_High_Ring_Prefab;
    public GameObject item_High_Necklace_Prefab;
    public GameObject item_High_Guitar_Prefab;
    public GameObject item_High_Monitor_Prefab;
    public GameObject item_High_Sushi_Prefab;

    //Dictionary로 풀 관리
    public Dictionary<PoolTag, Queue<GameObject>> pool = new Dictionary<PoolTag, Queue<GameObject>>();
    public Dictionary<PoolTag, GameObject> prefabs = new Dictionary<PoolTag, GameObject>();

    //특별히 렉돌의 부모
    public Transform unuseableRagdolls;

    public void Init(StartGame sg)
    {
        roamingAI = sg.roamingAI;
        ragdollPrefab = sg.ragdollPrefab;
        moneyPrefab = sg.moneyPrefab;
        lowBoxPrefab = sg.lowBoxPrefab;
        midBoxPrefab = sg.midBoxPrefab;
        highBoxPrefab = sg.highBoxPrefab;
        item_Low_Donut_Prefab = sg.item_Low_Donut_Prefab;
        item_Low_Hamburger_Prefab = sg.item_Low_Hamburger_Prefab;
        item_Low_Burrito_Prefab = sg.item_Low_Burrito_Prefab;
        item_Low_Pizza_Prefab = sg.item_Low_Pizza_Prefab;
        item_Low_Shoe_Prefab = sg.item_Low_Shoe_Prefab;
        item_Mid_Dog_Prefab = sg.item_Mid_Dog_Prefab;
        item_Mid_Cat_Prefab = sg.item_Mid_Cat_Prefab;
        item_Mid_Album_Prefab = sg.item_Mid_Album_Prefab;
        item_Mid_Banana_Prefab = sg.item_Mid_Banana_Prefab;
        item_Mid_Cake_Prefab = sg.item_Mid_Cake_Prefab;
        item_Mid_Pineapple_Prefab = sg.item_Mid_Pineapple_Prefab;
        item_Mid_Shoe_Prefab = sg.item_Mid_Shoe_Prefab;
        item_High_Ring_Prefab = sg.item_High_Ring_Prefab;
        item_High_Necklace_Prefab = sg.item_High_Necklace_Prefab;
        item_High_Guitar_Prefab = sg.item_High_Guitar_Prefab;
        item_High_Monitor_Prefab = sg.item_High_Monitor_Prefab;
        item_High_Sushi_Prefab = sg.item_High_Sushi_Prefab;

        //Instance = this;

        //프리펩
        prefabs.Add(PoolTag.RoamingAI, roamingAI);
        prefabs.Add(PoolTag.Ragdoll, ragdollPrefab);
        prefabs.Add(PoolTag.Box_Low, lowBoxPrefab);
        prefabs.Add(PoolTag.Box_Mid, midBoxPrefab);
        prefabs.Add(PoolTag.Box_High, highBoxPrefab);
        prefabs.Add(PoolTag.Money, moneyPrefab);

        prefabs.Add(PoolTag.Item_Low_Donut, item_Low_Donut_Prefab);
        prefabs.Add(PoolTag.Item_Low_Hamburger, item_Low_Hamburger_Prefab);
        prefabs.Add(PoolTag.Item_Low_Burrito, item_Low_Burrito_Prefab);
        prefabs.Add(PoolTag.Item_Low_Pizza, item_Low_Pizza_Prefab);
        prefabs.Add(PoolTag.Item_Low_Shoe, item_Low_Shoe_Prefab);

        prefabs.Add(PoolTag.Item_Mid_Dog, item_Mid_Dog_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Cat, item_Mid_Cat_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Album, item_Mid_Album_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Banana, item_Mid_Banana_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Cake, item_Mid_Cake_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Pineapple, item_Mid_Pineapple_Prefab);
        prefabs.Add(PoolTag.Item_Mid_Shoe, item_Mid_Shoe_Prefab);

        prefabs.Add(PoolTag.Item_High_Ring, item_High_Ring_Prefab);
        prefabs.Add(PoolTag.Item_High_Necklace, item_High_Necklace_Prefab);
        prefabs.Add(PoolTag.Item_High_Guitar, item_High_Guitar_Prefab);
        prefabs.Add(PoolTag.Item_High_Monitor, item_High_Monitor_Prefab);
        prefabs.Add(PoolTag.Item_High_Sushi, item_High_Sushi_Prefab);

        //풀
        pool.Add(PoolTag.RoamingAI, new Queue<GameObject>());
        //pool.Add(PoolTag.Ragdoll, new Queue<GameObject>());
        pool.Add(PoolTag.Box_Low, new Queue<GameObject>());
        pool.Add(PoolTag.Box_Mid, new Queue<GameObject>());
        pool.Add(PoolTag.Box_High, new Queue<GameObject>());
        pool.Add(PoolTag.Money, new Queue<GameObject>());

        pool.Add(PoolTag.Item_Low_Donut, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Low_Hamburger, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Low_Burrito, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Low_Pizza, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Low_Shoe, new Queue<GameObject>());

        pool.Add(PoolTag.Item_Mid_Dog, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Cat, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Album, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Banana, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Cake, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Pineapple, new Queue<GameObject>());
        pool.Add(PoolTag.Item_Mid_Shoe, new Queue<GameObject>());

        pool.Add(PoolTag.Item_High_Ring, new Queue<GameObject>());
        pool.Add(PoolTag.Item_High_Necklace, new Queue<GameObject>());
        pool.Add(PoolTag.Item_High_Guitar, new Queue<GameObject>());
        pool.Add(PoolTag.Item_High_Monitor, new Queue<GameObject>());
        pool.Add(PoolTag.Item_High_Sushi, new Queue<GameObject>());


        //생성
        foreach (var key in pool.Keys)
        {
            for (int i = 0; i < poolCount; i++)
            {
                pool[key].Enqueue(CreateNewObject(key));
            }
        }

        FindUnuseableRagdoll();

        ////삭제 ㄴㄴ
        //DontDestroyOnLoad(gameObject);
    }
    private void FindUnuseableRagdoll()
    {
        if(unuseableRagdolls == null)
            unuseableRagdolls = GameObject.FindGameObjectWithTag("unuseableRagdolls")?.transform;
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
        if (key == PoolTag.Ragdoll)
        {
            var ragdoll = Instantiate(prefabs[key]);
            return ragdoll;
        }
        else
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
                if (agent != null)
                {
                    agent.enabled = false;
                    //agent.isStopped = false;
                    agent.velocity = Vector3.zero;
                }
                return obj;
            }
            else
            {
                var newObj = CreateNewObject(key);
                newObj.SetActive(true);
                newObj.transform.SetParent(null);
                var agent = newObj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                    //agent.isStopped = false;
                    agent.velocity = Vector3.zero;
                }
                return newObj;
            }
        }
    }
    public void ReturnObject(PoolTag key, GameObject go)
    {
        if (key == PoolTag.Ragdoll)
        {
            go.SetActive(false);
            FindUnuseableRagdoll();
            go.transform.SetParent(unuseableRagdolls);
        }
        else
        {
            var queue = pool[key];
            go.transform.SetParent(transform);
            queue.Enqueue(go);
            var agent = go.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                agent.enabled = false;
            }
            go.SetActive(false);
        }
    }
}
