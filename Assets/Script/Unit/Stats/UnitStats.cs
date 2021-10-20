using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats.asset", menuName = "Unit/Stats")]
public class UnitStats : ScriptableObject
{
    public float speed;
    public int maximumMoney;
    public int maximumScore;
}