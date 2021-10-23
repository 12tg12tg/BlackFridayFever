using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private bool backToPool;
    [SerializeField] private float timer;
    [SerializeField] private float disableTime = 1.5f;

    public ItemInfo itemInfo;
    private void Update()
    {
        //시간 후 Pool로 보내기
        if(backToPool)
        {
            timer += Time.deltaTime;
            if(timer > disableTime)
            {
                //변수 초기화
                backToPool = false;
                timer = 0f;

                //Pool로 되돌리기
                switch (itemInfo.value)
                {
                    case ItemValue.Low:
                        GameObjectPool.Instance.ReturnObject(PoolTag.Box_Low, gameObject);
                        break;
                    case ItemValue.Mid:
                        GameObjectPool.Instance.ReturnObject(PoolTag.Box_Mid, gameObject);
                        break;
                    case ItemValue.High:
                        GameObjectPool.Instance.ReturnObject(PoolTag.Box_High, gameObject);
                        break;
                }
                
                //item 리스폰

            }
        }
    }
    public void Init(ItemInfo info)
    {
        itemInfo = info;
    }
    public void BoxToItem()
    {
        backToPool = true;
    }
}
