using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("(Inspector 수정 : 1개)")]
    public int itemStack;
    public int score;
    public int money;
    public float speed;
    public float loadUpTimer;
    public bool isStuned;
    public TruckScript truck;
    private Rigidbody rigid;
    [Header("(Inspector 연결) Unit → Stats 생성해서 넣기!")] public UnitStats stats;

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
        GetComponentInChildren<Animator>().SetTrigger("Push");          //애니실행하세요
        var target = truck.transform.position;
        target.y = transform.position.y;
        var lookTruckPos = target - transform.position;
        transform.rotation = Quaternion.LookRotation(lookTruckPos);     //트럭쳐다보세요
        if(GetComponent<PlayerController>() != null)
            Camera.main.GetComponent<CameraMove>().StartSaveLoadCamMove();  //카메라무브
        StartCoroutine(CoLoadUp());
    }
    private IEnumerator CoLoadUp() //점수더하기. 차에 싣기.
    {
        //점수반영
        truck.SavePurchased(score);
        score = 0;
        itemStack = 0;

        //종료인지 검사
        GameManager.GM.IsEnd(GetComponent<CharacterStats>());


        //짐 Freeze해제
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

        //하나씩 저장(Pool로 되돌려주기)
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

            //사운드재생
            if(GameManager.GM.State==GameManager.GameState.End ||
                stats.type == UnitType.Player)
                SoundManager.Instance.PlayLoadTruck();
            yield return new WaitForSeconds(term);
        }
        //itemStack = 0;
    }

    public void DropItem(Vector3 forceFoward)
    {
        //상자 Freeze해제 및 부모 null 및 AddForce
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var purchasedList = liftLoads.purchaseds;
        for (int i = 0; i < purchasedList.Count; i++)
        {
            //짐 Freeze해제 
            var rigid = purchasedList[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.None;


            //AddForce
            var forceFoward2 = Quaternion.Euler(0f, 0f, 90f) * forceFoward;
            var minRot = Quaternion.Euler(0f, -30f, 0f) * forceFoward2;
            var maxRot = Quaternion.Euler(0f, 30f, 0f) * forceFoward2;
            var force = Vector3.Lerp(minRot, maxRot, Random.Range(0f, 1f)).normalized;

            rigid.AddForce(force * 7f, ForceMode.Impulse);


            //부모 null
            purchasedList[i].transform.SetParent(null);
            //Box의 일정 시간 후 아이템으로 치환되며 사라지는 함수 호출.
            var box = purchasedList[i].GetComponent<Box>();
            box.BoxToItem();
        }

        //CharacterStats의 존재하는 수치 조정하기
        score = 0;
        itemStack = 0;
        purchasedList.Clear();

        //본체 에이전트 끄고 AddForce하기.
        var ai = GetComponent<AiBehaviour>();
        if (ai == null)
        {
            //플레이어
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

    //승리 애니메이션바꾸기
    public void EndAnimation(bool isWin)
    {
        var ai = GetComponent<AiBehaviour>();
        if (isWin)
        {
            if (ai == null)
            {
                //플레이어
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
            //본체 에이전트 끄고 AddForce하기.
            if (ai == null)
            {
                //플레이어
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
        //상자 Freeze해제 및 부모 null 및 AddForce
        LiftLoad liftLoads = GetComponentInChildren<LiftLoad>();
        var boxArr = liftLoads.purchaseds;
        for (int i = 0; i < boxArr.Count; i++)
        {
            //짐 Freeze해제 
            var rigid = boxArr[i].GetComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.None;
            var randomX = Random.Range(-3f, 3f);
            var randomZ = Random.Range(-3f, 3f);
            var force = new Vector3(randomX, 5, randomZ);
            rigid.AddForce(force.normalized * 20, ForceMode.Impulse);
            //Debug.Log("힘 개방!");
        }

        //liftload 콜라이더 해제
        liftLoads.GetComponent<Collider>().enabled = false;
    }
}

