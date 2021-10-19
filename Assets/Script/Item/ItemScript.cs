using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemInfo info;

    private void Awake()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            var stats = collision.gameObject.GetComponent<CharacterStats>();
            if(GameManager.GM.ItemCollsion(stats, info))
                gameObject.SetActive(false);
        }
    }
}
