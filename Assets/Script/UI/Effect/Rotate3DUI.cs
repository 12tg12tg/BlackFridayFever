using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate3DUI : MonoBehaviour
{
    public GameObject rewardPrefab; //아직 안씀. 프리펩 불러오기할때 자식으로 넣기.
    public float rotPerSecond;

    private void Awake()
    {
        /*자식으로 넣기*/
    }
    private void Update()
    {
        transform.localRotation *= Quaternion.Euler(0f, rotPerSecond * Time.deltaTime, 0f);
    }
}
