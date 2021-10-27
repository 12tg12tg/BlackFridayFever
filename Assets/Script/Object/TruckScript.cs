using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    [Header("(Inspector ���� : 1��)")]
    public int currentScore;
    public Transform dokingSpot;
    public Transform[] camPosForSave;
    [Header("(Inspector ����) BodyMesh�� ���� ���� �ش��ϴ� ������ MeshRenderer ����)")]
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
