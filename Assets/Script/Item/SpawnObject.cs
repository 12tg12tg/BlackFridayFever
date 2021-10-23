using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject myObject;
    private bool isRespawnDelay;
    private PoolTag curType;
    private float delay = 3f;
    private float timer;


    private void Start()
    {
        SpawnNewItem();
    }

    private void Update()
    {
        if(isRespawnDelay)
        {
            timer += Time.deltaTime;
            if(timer > delay)
            {
                timer = 0f;
                isRespawnDelay = false;
                SpawnNewItem();
            }
        }
    }

    public void SpawnNewItem()
    {
        switch (gameObject.tag)
        {
            case "Money":
                curType = PoolTag.Money;
                break;
            case "LowItem":
                curType = (PoolTag)Random.Range(GameObjectPool.Instance.LowMin, GameObjectPool.Instance.LowMax + 1);
                break;
            case "MidItem":
                curType = (PoolTag)Random.Range(GameObjectPool.Instance.MidMin, GameObjectPool.Instance.MidMax + 1);
                break;
            case "HighItem":
                curType = (PoolTag)Random.Range(GameObjectPool.Instance.HighMin, GameObjectPool.Instance.HighMax + 1);
                break;
        }

        myObject = GameObjectPool.Instance.GetObject(curType);
        myObject.transform.SetParent(transform);
        myObject.transform.localPosition = Vector3.zero;
    }

    public void SleepObject()
    {
        isRespawnDelay = true;
        GameObjectPool.Instance.ReturnObject(curType, myObject);
        myObject = null;
    }
}
