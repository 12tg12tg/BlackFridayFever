using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public RectTransform rotGo;
    public float rotPerSecond;
    private float oneCycleTime;
    private float timer;
    private bool isLeftTurn;

    private void Awake()
    {
        oneCycleTime = 360f / rotPerSecond;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > oneCycleTime)
        {
            timer = 0f;
            isLeftTurn = !isLeftTurn;
        }
        if(isLeftTurn)
            rotGo.rotation *= Quaternion.Euler(0f, 0f, rotPerSecond * Time.deltaTime);
        else
            rotGo.rotation *= Quaternion.Euler(0f, 0f, -rotPerSecond * Time.deltaTime);
    }
}
