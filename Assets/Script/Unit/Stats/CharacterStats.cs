using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int itemStack;
    public int score;
    public int money;
    public float speed;
    public UnitStats stats;
    private void Start()
    {
        speed = stats.speed;
    }
    public void getMoney(int money)
    {
        this.money += money;
    }
    public void getStack(int itemScore, int stack, int price)
    {
        money -= price;
        itemStack += stack;
        score += itemScore;
    }
    public void dropStack(int loseScore)
    {
        if (itemStack <= 0) return;

        /*물건 떨구는 구문*/

        //itemCount--;
        //score -= loseScore;

        //if(score < 0)
        //    score = 0;
    }
}
