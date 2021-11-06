using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("(Inspector ���� : 1��)")]
    public int itemStack;
    public int score;
    public int money;
    public float speed;
    public float loadUpTimer;
    public bool isStuned;
    public TruckScript truck;
    private Rigidbody rigid;
    [Header("(Inspector ����) Unit �� Stats �����ؼ� �ֱ�!")] public UnitStats stats;

    private void Awake()
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
        GetComponentInChildren<Animator>().SetTrigger("Push");          //�ִϽ����ϼ���
        var target = truck.transform.position;
        target.y = transform.position.y;
        var lookTruckPos = target - transform.position;
        transform.rotation = Quaternion.LookRotation(lookTruckPos);     //Ʈ���Ĵٺ�����
        if(GetComponent<PlayerController>() != null)
            Camera.main.GetComponent<CameraMove>().StartSaveLoadCamMove();  //ī�޶󹫺�
        StartCoroutine(CoLoadUp());
    }
    private IEnumerator CoLoadUp() //�������ϱ�. ���� �Ʊ�.
    {
        //�����ݿ�
        truck.SavePurchased(score);
        score = 0;
        itemStack = 0;

        //�������� �˻�
        GameManager.GM.IsEnd(GetComponent<CharacterStats>());


        //�� Freeze����
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var purchasedList = liftLoads.purchaseds;

        for (int i = 0; i < purchasedList.Count; i++)
        {
            var rigid = purchasedList[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }

        var count = purchasedList.Count;
        var foolTime = 2f;
        var term = foolTime / (count - 1);

        //�ϳ��� ����(Pool�� �ǵ����ֱ�)
        while (purchasedList.Count != 0)
        {
            var removeItem = purchasedList[0];
            var itemInfo = removeItem.GetComponent<Box>().itemInfo;
            switch (itemInfo.value)
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

            truck.GetComponentInChildren<LiftLoadTruck>().LiftPurchased(itemInfo);

            //�������
            if(GameManager.GM.State==GameManager.GameState.End ||
                stats.type == UnitType.Player)
                SoundManager.Instance.PlayLoadTruck();
            yield return new WaitForSeconds(term);
        }
        //itemStack = 0;
    }

    public void DropItem(Vector3 forceFoward)
    {
        //���� Freeze���� �� �θ� null �� AddForce
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var purchasedList = liftLoads.purchaseds;
        for (int i = 0; i < purchasedList.Count; i++)
        {
            //�� Freeze���� 
            var rigid = purchasedList[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.None;


            //AddForce
            var forceFoward2 = Quaternion.Euler(0f, 0f, 90f) * forceFoward;
            var minRot = Quaternion.Euler(0f, -30f, 0f) * forceFoward2;
            var maxRot = Quaternion.Euler(0f, 30f, 0f) * forceFoward2;
            var force = Vector3.Lerp(minRot, maxRot, Random.Range(0f, 1f)).normalized;

            rigid.AddForce(force * 7f, ForceMode.Impulse);


            //�θ� null
            purchasedList[i].transform.SetParent(null);
            //Box�� ���� �ð� �� ���������� ġȯ�Ǹ� ������� �Լ� ȣ��.
            var box = purchasedList[i].GetComponent<Box>();
            box.BoxToItem();
        }

        //CharacterStats�� �����ϴ� ��ġ �����ϱ�
        score = 0;
        itemStack = 0;
        purchasedList.Clear();

        //��ü ������Ʈ ���� AddForce�ϱ�.
        var ai = GetComponent<AiBehaviour>();
        if (ai == null)
        {
            //�÷��̾�
            var player = GetComponent<PlayerController>();
            player?.CrushInit();
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

    //�¸� �ִϸ��̼ǹٲٱ�
    public void EndAnimation(bool isWin)
    {
        var ai = GetComponent<AiBehaviour>();
        if (isWin)
        {
            if (ai == null)
            {
                //�÷��̾�
                var player = GetComponent<PlayerController>();
                player.SetWinAnimation();
            }
            else
            {
                //Ai
                ai.CrushInit();
                ai.SetWinAnimation();
            }
        }
        else
        {
            //��ü ������Ʈ ���� AddForce�ϱ�.
            if (ai == null)
            {
                //�÷��̾�
                var player = GetComponent<PlayerController>();
                player.SetDeafeatAnimation();
            }
            else
            {
                //Ai
                ai.CrushInit();
                ai.SetDeafeatAnimation();
            }
        }
    }

    public void BoxUnFreeze()
    {
        //���� Freeze���� �� �θ� null �� AddForce
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var boxArr = liftLoads.purchaseds;
        for (int i = 0; i < boxArr.Count; i++)
        {
            //�� Freeze���� 
            var rigid = boxArr[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.None;
            var randomX = Random.Range(-3f, 3f);
            var randomZ = Random.Range(-3f, 3f);
            var force = new Vector3(randomX, 5, randomZ);
            rigid.AddForce(force.normalized * 20, ForceMode.Impulse);
            //Debug.Log("�� ����!");
        }

        //liftload �ݶ��̴� ����
        liftLoads.GetComponent<Collider>().enabled = false;
    }
}

