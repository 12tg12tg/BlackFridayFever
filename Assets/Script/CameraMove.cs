using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform player;
    public float offsetZ = 5f;
    public float offsetY = 40f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //�ʱ�ī�޶���ġ����
        transform.position = player.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
    }
    private void Update()
    {
        transform.position = player.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
        transform.LookAt(player);
    }
}
