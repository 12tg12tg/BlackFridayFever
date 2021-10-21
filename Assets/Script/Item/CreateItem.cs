using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
    public List<GameObject> highItemsPrefab;
    public List<GameObject> midItemsPrefab;
    public List<GameObject> lowItemsPrefab;

    public GameObject[] lowItem;
    public GameObject[] midItem;
    public GameObject[] HighItem;
    private void Start()
    {
        lowItem = GameObject.FindGameObjectsWithTag("LowItem");
        for (int i = 0; i < lowItem.Length; i++)
        {
            int rand = Random.Range(0, highItemsPrefab.Count);
            Instantiate(lowItemsPrefab[rand], lowItem[i].transform);
        }

        midItem = GameObject.FindGameObjectsWithTag("MidItem");
        for (int i = 0; i < midItem.Length; i++)
        {
            int rand = Random.Range(0, midItemsPrefab.Count);
            Instantiate(midItemsPrefab[rand], midItem[i].transform);
        }

        HighItem = GameObject.FindGameObjectsWithTag("HighItem");
        for (int i = 0; i < HighItem.Length; i++)
        {
            int rand = Random.Range(0, highItemsPrefab.Count);
            Instantiate(highItemsPrefab[rand], HighItem[i].transform);
        }
    }
}
