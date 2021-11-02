using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("(Inspector ���� : 5�� / GoalScore���� CamLook����)")]
    //���������� �ʿ��� ��ü��
    public int goalScore;

    public CharacterStats[] Ais;
    public TruckScript[] trucks;

    public Transform[] startCamPos;
    public Transform[] startCamLook;

    public int StageNum;

    [Header("(Inspector ���� ���ʿ�)")]
    public float scanTime = 2.5f;
    public float toPlayerTime = 2.5f;




    private static Stage instance;
    public static Stage Instance
    {
        get { return instance; }
    }

    public GameObject[] moneys;
    public GameObject[] LowItems;
    public GameObject[] MidItems;
    public GameObject[] HighItems;

    private void Awake()
    {
        instance = this;
        moneys = GameObject.FindGameObjectsWithTag("Money");
        LowItems = GameObject.FindGameObjectsWithTag("LowItem");
        MidItems = GameObject.FindGameObjectsWithTag("MidItem");
        HighItems = GameObject.FindGameObjectsWithTag("HighItem");
    }
}
