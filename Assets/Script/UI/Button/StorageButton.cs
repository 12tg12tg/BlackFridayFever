using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageButton : MonoBehaviour
{
    public Button button;

    public Sprite secreteSprite;
    public Sprite nonSelectedSprite;
    public Sprite selectedSprite;
    public Sprite redSprite;

    public GameObject skinPrefab;
    public Image skinImg;
    public Sprite skinSprite;

    public bool isOpened;
    public bool isSelectedSkin;
    public bool isBuy;

    public StorageButtonGroup buttonGroup;

    public void OnClick()
    {
        SoundManager.Instance.PlayButtonClick();

        if (isOpened)
        {
            if (isBuy)
            {
                buttonGroup.SelectedButtonReset(this);
            }
            else
            {
                buttonGroup.FocusedButtonReset(this);
            }
        }
        else
        {
            buttonGroup.FocusedButtonReset(this);
        }
    }


    private void Update()
    {
        if(isOpened)
        {
            skinImg.enabled = true;
            if (isBuy)
            {
                if (isSelectedSkin)
                {
                    button.image.sprite = selectedSprite;
                }
                else
                {
                    button.image.sprite = nonSelectedSprite;
                }
            }
            else
            {
                button.image.sprite = redSprite;
            }
        }
        else
        {
            skinImg.enabled = false;
            button.image.sprite = secreteSprite;
        }
    }
}
