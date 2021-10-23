using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemInfo info;

    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            var stats = other.gameObject.GetComponent<CharacterStats>();
            if (!stats.isStuned && GameManager.GM.ItemCollsion(stats, info))
                GetComponentInParent<SpawnObject>().SleepObject();
        }
    }
}
