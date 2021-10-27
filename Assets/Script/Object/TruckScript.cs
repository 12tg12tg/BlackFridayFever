using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    [Header("(Inspector 수정 : 1개)")]
    public int currentScore;
    public Transform dokingSpot;
    public Transform[] camPosForSave;
    [Header("(Inspector 연결) BodyMesh에 차량 색에 해당하는 파츠의 MeshRenderer 연결)")]
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
