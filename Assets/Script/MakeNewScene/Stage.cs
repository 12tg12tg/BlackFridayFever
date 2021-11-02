using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("(Inspector 수정 : 5개 / GoalScore부터 CamLook까지)")]
    //스테이지에 필요한 객체들
    public int goalScore;

    public CharacterStats[] Ais;
    public TruckScript[] trucks;

    public Transform[] startCamPos;
    public Transform[] startCamLook;

    public int StageNum;

    [Header("(Inspector 수정 불필요)")]
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
