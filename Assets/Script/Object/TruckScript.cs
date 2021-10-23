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
    public void SavePurchased(int score)
    {
        currentScore += score;
        //Debug.Log("Save!");
    }
}
