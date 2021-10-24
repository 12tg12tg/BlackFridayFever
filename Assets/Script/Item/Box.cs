using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private bool backToPool;
    [SerializeField] private float timer;
    private float disableTime = 5f;

    public ItemInfo itemInfo;
    private void Update()
    {
        //�ð� �� Pool�� ������
        if(backToPool)
        {
            timer += Time.deltaTime;
            if(timer > disableTime)
            {
                //���� �ʱ�ȭ
                backToPool = false;
                timer = 0f;

                //item ������
                var go = GameObjectPool.Instance.GetObject(itemInfo.poolTag);
                go.transform.position = transform.position;


                //Pool�� �ǵ�����
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
