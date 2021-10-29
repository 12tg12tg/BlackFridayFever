using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate3DUI : MonoBehaviour
{
    public GameObject rewardPrefab; //���� �Ⱦ�. ������ �ҷ������Ҷ� �ڽ����� �ֱ�.
    public float rotPerSecond;

    private void Awake()
    {
        /*�ڽ����� �ֱ�*/
    }
    private void Update()
    {
        transform.localRotation *= Quaternion.Euler(0f, rotPerSecond * Time.deltaTime, 0f);
    }
}
