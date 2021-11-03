using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int money;
    public float rotToSec;

    private void Start()
    {

    }
    private void Update()
    {
        var rot = rotToSec * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(0f, rot, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit") && 
            other.GetComponent<CharacterStats>().itemStack == 0 &&
            other.GetComponent<RoamingAiBehaviour>() == null)
        {
            var stats = other.gameObject.GetComponent<CharacterStats>();
            GameManager.GM.MoneyCollision(stats, this);
            GetComponentInParent<SpawnObject>().SleepObject();
        }
    }
}
