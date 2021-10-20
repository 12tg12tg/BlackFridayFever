using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GaveState
    {
        Start,
        Play,
        End,
    }
    //Singleton
    private static GameManager instance;
    public static GameManager GM
    {
        get { return instance; }
    }

    //UI
    public Text score;
    public Text money;
    public Text stack;
    public Text gauage;

    //Player
    public GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        instance = this;
    }
    private void Update()
    {
        
    }

    //��
    public void MoneyCollision(CharacterStats unit, Money money)
    {
        unit.getMoney(money.money);
        if (unit.tag == "Player")
        {
            this.money.text = unit.money.ToString();
        }
    }


    //���� & ��ǰ
    public bool ItemCollsion(CharacterStats unit, ItemInfo item)
    {
        //�� üũ
        if (unit.money >= item.price)
        {
            //���� ����� ��.
            int stack;
            if (item.value == ItemValue.Hight)
                stack = 3;
            else if (item.value == ItemValue.Mid)
                stack = 2;
            else
                stack = 1;

            unit.getStack(item.itemScore, stack, item.price);//�����ø���
            unit.GetComponentInChildren<LiftLoad>().LiftPurchased(0); //���ǿø���
            //item.SetActive(false);  //��Ȱ��ȭ

            if (unit.tag == "Player")
            {
                score.text = unit.score.ToString();
                money.text = unit.money.ToString();
                this.stack.text = unit.itemStack.ToString();
                gauage.text = unit.score.ToString("P1");
            }
            return true;
        }
        return false;
    }

}
