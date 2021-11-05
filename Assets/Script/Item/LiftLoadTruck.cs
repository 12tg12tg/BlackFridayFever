using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLoadTruck : MonoBehaviour
{
    public MeshRenderer[] mesh; //bounds찾는 용
    public Collider stackPosCol;
    //public List<GameObject> purchaseds;

    private void Awake()
    {
        stackPosCol = GetComponent<Collider>();
    }
    public void LiftPurchased(ItemInfo info)
    {
        //상자 찾기
        GameObject go = null;

        switch (info.value)
        {
            case ItemValue.Low:
                go = GameObjectPool.Instance.GetObject(PoolTag.Box_Low);
                break;
            case ItemValue.Mid:
                go = GameObjectPool.Instance.GetObject(PoolTag.Box_Mid);
                break;
            case ItemValue.High:
                go = GameObjectPool.Instance.GetObject(PoolTag.Box_High);
                break;
        }

        go.GetComponent<Box>().Init(info);

        var maxY = 0f;
        mesh = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            var bounds = mesh[i].bounds;
            //Vector3 extents = bounds.extents;
            var y = mesh[i].bounds.max.y;
            maxY = maxY > y ? maxY : y;
        }

        var pos = stackPosCol.bounds.center;

        if (maxY != 0)
            pos.y = maxY;
        else
            pos.y += stackPosCol.bounds.extents.y;

        go.SetActive(true);
        go.transform.SetParent(transform);
        go.transform.position = pos;
        //purchaseds.Add(go);
    }

    ////Test
    //public void PutLowVal()
    //{
    //    ItemInfo info = new ItemInfo();
    //    info.value = ItemValue.Low;
    //    LiftPurchased(info);
    //}
    //public void PutMidVal()
    //{
    //    ItemInfo info = new ItemInfo();
    //    info.value = ItemValue.Mid;
    //    LiftPurchased(info);
    //}
    //public void PutHighVal()
    //{
    //    ItemInfo info = new ItemInfo();
    //    info.value = ItemValue.High;
    //    LiftPurchased(info);
    //}
}
