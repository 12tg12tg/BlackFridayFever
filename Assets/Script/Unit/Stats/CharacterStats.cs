using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int itemStack;
    public int score;
    public int money;
    public float speed;
    public float loadUpTimer;
    public bool isStuned;
    public UnitStats stats;
    public TruckScript truck;
    private Rigidbody rigid;

    private void Start()
    {
        speed = stats.speed;
        rigid = GetComponent<Rigidbody>();
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
    }
    public void LoadUp()
    {
        GetComponentInChildren<Animator>().SetTrigger("Push");
        StartCoroutine(CoLoadUp());
    }
    private IEnumerator CoLoadUp() //차에 싣기.
    {
        //점수반영
        truck.SavePurchased(score);
        score = 0;


        //짐 Freeze해제
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var purchasedList = liftLoads.purchaseds;

        for (int i = 0; i < purchasedList.Count; i++)
        {
            var rigid = purchasedList[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        //하나씩 저장(Pool로 되돌려주기)
        while (purchasedList.Count != 0)
        {
            var removeItem = purchasedList[0];

            switch (removeItem.GetComponent<Box>().itemInfo.value)
            {
                case ItemValue.Low:
                    GameObjectPool.Instance.ReturnObject(PoolTag.Box_Low, removeItem.gameObject);
                    break;
                case ItemValue.Mid:
                    GameObjectPool.Instance.ReturnObject(PoolTag.Box_Mid, removeItem.gameObject);
                    break;
                case ItemValue.High:
                    GameObjectPool.Instance.ReturnObject(PoolTag.Box_High, removeItem.gameObject);
                    break;
            }
            removeItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            purchasedList.RemoveAt(0);
            yield return new WaitForSeconds(0.2f);
        }

        itemStack = 0;
    }

    public void DropItem(Vector3 forceFoward)
    {
        //짐 Freeze해제 및 부모 null
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var purchasedList = liftLoads.purchaseds;
        for (int i = 0; i < purchasedList.Count; i++)
        {
            //짐 Freeze해제
            var rigid = purchasedList[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.None;
            //부모 null
            purchasedList[i].transform.SetParent(null);
            //Box의 일정 시간 후 아이템으로 치환되며 사라지는 함수 호출.
            var box = purchasedList[i].GetComponent<Box>();
            box.BoxToItem();
        }

        //CharacterStats의 존재하는 수치 조정하기
        score = 0;
        itemStack = 0;

        //본체 에이전트 끄고 AddForce하기.
        var ai = GetComponent<AiBehaviour>();
        if (ai == null)
        {
            //플레이어
            var player = GetComponent<PlayerController>();
            player.CrushInit();
        }
        else
        {
            //Ai
            ai.CrushInit();
        }
        transform.rotation = Quaternion.LookRotation(-forceFoward);
        rigid.AddForce(forceFoward * 7f, ForceMode.Impulse);
        isStuned = true;
    }
}
