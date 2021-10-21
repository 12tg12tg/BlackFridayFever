using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    public int currentScore;
    public Transform dokingSpot;

    private void Start()
    {

    }
    public void SavePurchased(ItemInfo item)
    {
        currentScore += item.itemScore;
        //Debug.Log("Save!");
    }
}
