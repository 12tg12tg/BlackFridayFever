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


    private void Awake()
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
                curType = Stage.Instance.RandLow;
                break;
            case "MidItem":
                curType = Stage.Instance.RandMid;
                break;
            case "HighItem":
                curType = Stage.Instance.RandHigh;
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
