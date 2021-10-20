using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int money;
    private MeshRenderer mesh;
    private new Collider collider;
    private Rigidbody rigid;
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
            if(cantake)
            {
                mesh.enabled = true;
                collider.enabled = true;
                rigid.useGravity = true;
            }
            else
            {
                mesh.enabled = false;
                collider.enabled = false;
                rigid.useGravity = false;
            }
        }
    }
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody>();
        CanTake = true;
    }
    private float respawnTime = 3f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            var stats = collision.gameObject.GetComponent<CharacterStats>();
            GameManager.GM.MoneyCollision(stats, this);
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
