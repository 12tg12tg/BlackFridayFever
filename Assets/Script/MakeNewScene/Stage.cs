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
    public float toPlayerTime = 1f;


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
    public void Init()
    {
        if (GameManager.GM.IsRandStage)
        {
            randomAiIndexArr = GameManager.GM.randStageInfo.randAiIndex;
            DeleteAndResetAIs();
        }
    }

    public PoolTag[] lowValues;
    public PoolTag[] MidValues;
    public PoolTag[] HighValues;

    public PoolTag RandLow
    {
        get
        {
            return lowValues[Random.Range(0, lowValues.Length)];
        }
    }
    public PoolTag RandMid
    {
        get
        {
            return MidValues[Random.Range(0, MidValues.Length)];
        }
    }
    public PoolTag RandHigh
    {
        get
        {
            return HighValues[Random.Range(0, HighValues.Length)];
        }
    }


    public GameObject[] randomPrefAIs;

    private int[] randomAiIndexArr;
    public int[] RandAiIndexArr
    {
        get
        {
            int[] arr = new int[3];
            for (int i = 0; i < 3; i++)
            {
                arr[i] = Random.Range(0, randomPrefAIs.Length);
            }
            return arr;
        }
    }

    public void DeleteAndResetAIs()
    {
        for (int i = 0; i < Ais.Length; i++)
        {
            var prefab = randomPrefAIs[randomAiIndexArr[i]];
            var newGo = Instantiate(prefab, transform);
            newGo.name = $"(random){i}_{prefab.name}";
            Ais[i].gameObject.SetActive(false);
            Ais[i] = newGo.GetComponent<CharacterStats>();
        }
    }
}
