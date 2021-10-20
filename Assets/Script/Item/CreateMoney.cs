using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMoney : MonoBehaviour
{
    public GameObject moneyPrefab;
    public GameObject[] moneySpots;
    private void Start()
    {
        moneySpots = GameObject.FindGameObjectsWithTag("Money");
        for (int i = 0; i < moneySpots.Length; i++)
        {
            Instantiate(moneyPrefab, moneySpots[i].transform);
        }
    }
}
