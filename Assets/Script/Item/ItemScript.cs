using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemInfo info;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            var stats = collision.gameObject.GetComponent<CharacterStats>();
            stats.getStack(info.itemScore);
            gameObject.SetActive(false);
        }
    }
}
