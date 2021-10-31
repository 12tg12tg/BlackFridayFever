using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    private Sprite originalImg;
    public Sprite checkedImg;
    private Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        originalImg = toggle.image.sprite;
    }
    public void ChangeImage(bool isOn)
    {
        if (isOn)
        {
            toggle.image.sprite = checkedImg;
            Debug.Log("체크");
        }
        else
        {
            toggle.image.sprite = originalImg;
            Debug.Log("해제");
        }
    }
}
