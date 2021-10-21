using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPool : MonoBehaviour
{
    public static BoxPool Instance;
    public GameObject lowValueBoxPrefab;
    public GameObject midValueBoxPrefab;
    public GameObject highValueBoxPrefab;

    private Queue<GameObject> lowValPool = new Queue<GameObject>();
    private Queue<GameObject> midValPool = new Queue<GameObject>();
    private Queue<GameObject> highValPool = new Queue<GameObject>();

    private int poolCount = 100;
    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolCount; i++)
        {
            lowValPool.Enqueue(CreateNewObject(lowValueBoxPrefab));
            midValPool.Enqueue(CreateNewObject(midValueBoxPrefab));
            highValPool.Enqueue(CreateNewObject(highValueBoxPrefab));
        }
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        var newGo = Instantiate(prefab);
        newGo.transform.SetParent(transform);
        newGo.SetActive(false);
        return newGo;
    }

    public GameObject GetLowValueBox()
    {
        if (lowValPool.Count > 0)
        { 
            var obj = lowValPool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj; 
        }
        else
        {
            var newObj = CreateNewObject(lowValueBoxPrefab);
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public GameObject GetMidValueBox()
    {
        if (midValPool.Count > 0)
        {
            var obj = Instance.midValPool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(midValueBoxPrefab);
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public GameObject GetHighValueBox()
    {
        if (highValPool.Count > 0)
        {
            var obj = highValPool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(highValueBoxPrefab);
            newObj.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public void ReturnLowValue(GameObject low)
    {
        low.SetActive(false);
        low.transform.SetParent(transform);
        lowValPool.Enqueue(low);
    }
    public void ReturnMidValue(GameObject mid)
    {
        mid.SetActive(false);
        mid.transform.SetParent(transform);
        lowValPool.Enqueue(mid);
    }
    public void ReturnHighValue(GameObject high)
    {
        high.SetActive(false);
        high.transform.SetParent(transform);
        lowValPool.Enqueue(high);
    }
}
