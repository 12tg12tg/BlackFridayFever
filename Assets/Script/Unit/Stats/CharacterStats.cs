using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public List<ItemInfo> belongings;
    public int itemStack;
    public int score;
    public int money;
    public float speed;
    public UnitStats stats;
    public float loadUpTimer;
    public TruckScript truck;
    private void Start()
    {
        speed = stats.speed;
    }
    public void getMoney(int money)
    {
        this.money += money;
    }
    public void getStack(ItemInfo item)
    {
        money -= item.price;
        itemStack += (int)item.value + 1;
        score += item.itemScore;
        belongings.Add(item);
    }
    public void LoadUp()
    {
        GetComponentInChildren<Animator>().SetTrigger("Push");
        StartCoroutine(CoLoadUp());
    }
    private IEnumerator CoLoadUp()
    {
        //yield return new WaitForSeconds(1f);

        while (belongings.Count != 0)
        {
            var item = belongings[0];
            truck.SavePurchased(item);
            score -= item.itemScore;

            List<Collider> temp = new List<Collider>();
            GetComponentsInChildren<Collider>(temp);
            for (int i = 0; i < temp.Count; )
            {
                if (temp[i].tag != "Purchased")
                    temp.RemoveAt(i);
                else
                    ++i;
            }

            switch (belongings[0].value)
            {
                case ItemValue.Low:
                    BoxPool.Instance.ReturnLowValue(temp[0].gameObject);
                    break;
                case ItemValue.Mid:
                    BoxPool.Instance.ReturnMidValue(temp[0].gameObject);
                    break;
                case ItemValue.High:
                    BoxPool.Instance.ReturnHighValue(temp[0].gameObject);
                    break;
            }
            belongings.RemoveAt(0);
            yield return new WaitForSeconds(0.2f);
        }

        itemStack = 0;
    }
    public void DropItem()
    {
        //모든 아이템을 부모를 null로
        List<Collider> temp = new List<Collider>();
        GetComponentsInChildren<Collider>(temp);
        foreach (var col in temp)
        {
            col.transform.SetParent(null);
        }

        //stats의 존재하는 수치 조정하기


        //3초후에 해당위치에 아이템으로 변화시키고 끄기.


        //본체 에이전트 끄고 AddForce하기.


    }
}
