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
        //��� �������� �θ� null��
        List<Collider> temp = new List<Collider>();
        GetComponentsInChildren<Collider>(temp);
        foreach (var col in temp)
        {
            col.transform.SetParent(null);
        }

        //stats�� �����ϴ� ��ġ �����ϱ�


        //3���Ŀ� �ش���ġ�� ���������� ��ȭ��Ű�� ����.


        //��ü ������Ʈ ���� AddForce�ϱ�.


    }
}
