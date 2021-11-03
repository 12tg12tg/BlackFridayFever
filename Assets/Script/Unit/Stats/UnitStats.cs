using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Ai,
    Ai_Money,
    Player,
    Ai_Tank,
    Ai_Tank2
}

[CreateAssetMenu(fileName = "UnitStats.asset", menuName = "Unit/Stats")]
public class UnitStats : ScriptableObject
{
    public UnitType type;
    public float speed;
    public float stunTime;
    public int maximumMoney;
    public int maximumScore;
    public float lowValueProp = 0f;
    public float midValueProp = 0f;
}