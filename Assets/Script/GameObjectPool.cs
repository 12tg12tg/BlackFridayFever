using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolTag
{
    Box_Low,
    Box_Mid,
    Box_High,
    Itme_Low_,
    Item_Mid_,
    Item_High_,

}

public class GameObjectPool : MonoBehaviour
{
    private int poolCount = 30;
    public static GameObjectPool Instance;
    public GameObject lowBoxPrefab;
    public GameObject midBoxPrefab;
    public GameObject highBoxPrefab;

    //Dictionary肺 钱 包府
    public Dictionary<PoolTag, Queue<GameObject>> pool = new Dictionary<PoolTag, Queue<GameObject>>();
    public Dictionary<PoolTag, GameObject> prefabs = new Dictionary<PoolTag, GameObject>();
    private void Awake()
    {
        Instance = this;

        //橇府崎
        prefabs.Add(PoolTag.Box_Low, lowBoxPrefab);
        prefabs.Add(PoolTag.Box_Mid, midBoxPrefab);
        prefabs.Add(PoolTag.Box_High, highBoxPrefab);
        //prefabs.Add(PoolTag.Box_High, highBoxPrefab);
        

        //钱
        pool.Add(PoolTag.Box_Low, new Queue<GameObject>());
        pool.Add(PoolTag.Box_Mid, new Queue<GameObject>());
        pool.Add(PoolTag.Box_High, new Queue<GameObject>());
        //pool.Add(PoolTag.High_, new Queue<GameObject>());


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
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(key);
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public void ReturnObject(PoolTag key, GameObject go)
    {
        var queue = pool[key];
        go.SetActive(false);
        go.transform.SetParent(transform);
        queue.Enqueue(go);
    }

}
