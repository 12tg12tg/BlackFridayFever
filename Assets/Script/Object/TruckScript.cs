using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    public int currentScore;
    public Transform dokingSpot;
    public Transform[] camPosForSave;
    public MeshRenderer bodyMesh;
    public Color bodyColor;

    private void Awake()
    {
        bodyColor = bodyMesh.material.color;
    }
    public void SavePurchased(int score)
    {
        currentScore += score;
        //Debug.Log("Save!");
    }
}
