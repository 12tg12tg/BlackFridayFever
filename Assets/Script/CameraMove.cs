using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;
    public float offsetZ;
    public float offsetY;


    public void Init()
    {
        //초기카메라위치설정
        transform.position = player.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
    }

    private void LateUpdate()
    {
        switch (GameManager.GM.State)
        {
            case GameManager.GameState.Idle:
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:
                transform.position = player.position - Vector3.forward * offsetZ + Vector3.up * offsetY;
                transform.LookAt(player);
                break;
            case GameManager.GameState.End:
                break;
        }
    }
}
