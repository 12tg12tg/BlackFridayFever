using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform player;
    public float offsetZ;
    public float offsetY;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //�ʱ�ī�޶���ġ����
        transform.position = player.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
    }
    private void LateUpdate()
    {
        transform.position = player.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
        transform.LookAt(player);
    }
}
