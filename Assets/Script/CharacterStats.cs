using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats.asset", menuName = "CharacterStats/BaseStats")]
public class CharacterStats : ScriptableObject
{
    public float speed;
    public int cash;
}
