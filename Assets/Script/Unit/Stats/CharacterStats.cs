using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int itemCount;
    public int score;
    public float speed;
    public UnitStats stats;
    private void Start()
    {
        speed = stats.speed;
    }

    public void getStack(int itemScore)
    {
        itemCount++;
        score += itemScore;
    }
    public void dropStack(int loseScore)
    {
        if (itemCount <= 0) return;

        itemCount--;
        score -= loseScore;

        if(score < 0)
            score = 0;
    }
}
