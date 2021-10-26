using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    //스테이지에 필요한 객체들
    [Header("가나다")]
    public int goalScore;

    public CharacterStats[] Ais;
    public TruckScript[] trucks;

    public Transform[] startCamPos;
    public Transform[] startCamLook;

    public float scanTime = 2.5f;
    public float toPlayerTime = 2.5f;
}
