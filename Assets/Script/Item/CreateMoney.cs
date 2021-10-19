using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMoney : MonoBehaviour
{
    public GameObject moneyPrefab;
    private void Start()
    {
        var transforms = GameObject.FindGameObjectsWithTag("Money");
        for (int i = 0; i < transforms.Length; i++)
        {
            Instantiate(moneyPrefab, transforms[i].transform);
        }
    }
}
