using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    public Sprite originalImg;
    public Sprite checkedImg;
    public Toggle toggle;

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
