using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemInfo info;
    private float rotToSec;
    private GameObject parentParticle;

    private void Awake()
    {
        rotToSec = 90f;
        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in particles)
        {
            if(particle.gameObject.tag == "ParentParticle")
            {
                parentParticle = particle.gameObject;
            }
        }
    }

    private void Update()
    {
        var rot = rotToSec * Time.deltaTime;
        transform.rotation *= Quaternion.Euler(0f, rot, 0f);
        parentParticle.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit") &&
            other.GetComponent<RoamingAiBehaviour>() == null)
        {
            var stats = other.gameObject.GetComponent<CharacterStats>();
            if (!stats.isStuned && GameManager.GM.ItemCollsion(stats, info))
            {
                var spawn = GetComponentInParent<SpawnObject>();
                if (spawn != null)
                    spawn.SleepObject();
                else
                    GameObjectPool.Instance.ReturnObject(info.poolTag, gameObject);
            }

        }
    }
}
