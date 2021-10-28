using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiRagdoll : MonoBehaviour
{
    public Rigidbody spine;
    private float timer;
    private float disapearTime = 3f;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > disapearTime)
        {
            GameObjectPool.Instance.ReturnObject(PoolTag.Ragdoll, gameObject);
        }
    }
}
