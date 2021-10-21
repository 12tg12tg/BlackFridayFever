using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemInfo info;
    private MeshRenderer mesh;
    private new Collider collider;
    //private Rigidbody rigid;
    private float respawnTime = 3f;
    private bool cantake;
    public bool CanTake
    {
        get
        {
            return cantake;
        }
        set
        {
            cantake = value;
            if (cantake)
            {
                mesh.enabled = true;
                collider.enabled = true;
                //rigid.useGravity = true;
            }
            else
            {
                mesh.enabled = false;
                collider.enabled = false;
                //rigid.useGravity = false;
            }
        }
    }
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        //rigid = GetComponent<Rigidbody>();
        CanTake = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            var stats = other.gameObject.GetComponent<CharacterStats>();
            if (GameManager.GM.ItemCollsion(stats, info))
                StartCoroutine(WaitRespawn());
        }
    }
    private IEnumerator WaitRespawn()
    {
        CanTake = false;
        yield return new WaitForSeconds(respawnTime);
        CanTake = true;
    }
}
