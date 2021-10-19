using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int money;
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
        var mesh = GetComponent<MeshRenderer>();
        var collider = GetComponent<Collider>();
        var rigid = GetComponent<Rigidbody>();
        mesh.enabled = false;
        collider.enabled = false;
        rigid.useGravity = false;
        yield return new WaitForSeconds(respawnTime);
        mesh.enabled = true;
        collider.enabled = true;
        rigid.useGravity = true;
    }
}
