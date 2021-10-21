using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLoad : MonoBehaviour
{
    public MeshRenderer[] mesh;
    public GameObject prefab;

    public void LiftPurchased(ItemValue value)
    {
        var maxY = 0f;
        mesh = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            var bounds = mesh[i].bounds;
            Vector3 extents = bounds.extents;
            var y = mesh[i].transform.position.y + extents.y * 2;
            maxY = maxY > y ? maxY : y;
        }

        var pos = transform.position;
        if (maxY != 0)
            pos.y = maxY;

        GameObject go = null;

        switch (value)
        {
            case ItemValue.Low:
                go = BoxPool.Instance.GetLowValueBox();
                break;
            case ItemValue.Mid:
                go = BoxPool.Instance.GetMidValueBox();
                break;
            case ItemValue.High:
                go = BoxPool.Instance.GetHighValueBox();
                break;
        }

        go.SetActive(true);
        go.transform.SetParent(transform);
        go.transform.position = pos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Tray") &&
            GetComponent<Collider>().GetHashCode() < collision.collider.GetHashCode())
        {
            GameManager.GM.DecideTrayCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
