using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageButtonGroup : GenericWindow
{
    public StorageButton[] buttons;
    public Button AD_Link;
    public Button buy500;

    public StorageButton curSelectedButton;
    public StorageButton curFocusButton;

    public int remainSkin;

    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].isOpened)
                remainSkin++;
        }
        ADButtonOnOFF();
        buy500.interactable = false;
    }

    private void Update()
    {
        ADButtonOnOFF();
        BuyOnOFF();
    }

    public void SelectedButtonReset(StorageButton button)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i] == button)
            {
                buttons[i].isSelectedSkin = !buttons[i].isSelectedSkin;
                if(buttons[i].isSelectedSkin)
                {
                    curSelectedButton = button;
                }
                else
                {
                    curSelectedButton = null;
                }
            }
            else
            {
                buttons[i].isSelectedSkin = false;
            }
        }
        curFocusButton = null;
    }

    public void FocusedButtonReset(StorageButton button)
    {
        curFocusButton = button;
    }


    public void ADButtonOnOFF()
    {
        if(remainSkin == 0)
        {
            AD_Link.interactable = false;
        }
        else
        {
            AD_Link.interactable = true;
        }
    }

    public void BuyOnOFF()
    {
        if (curFocusButton != null)
        {
            if (!curFocusButton.isOpened)
            {
                buy500.interactable = true;
                return;
            }
        }
        buy500.interactable = false;
    }

    public void BuySkin()
    {
        SoundManager.Instance.PlayButtonClick();
        /*static 전역 Save 구조체에서 금액 확인 후 차감*/
        if (true/*금액 비교 구문*/)
        {
            //금액이 충분하다면 차감하여 스킨얻기
            curFocusButton.isOpened = true;
        }
        else
        {
            //금액이 모자라다면 아무일도 일어나지 않는다. (혹은, 보석 상자 진동)

        }
    }

    public void WatchAdForSkin()
    {
        SoundManager.Instance.PlayButtonClick();

        /*광고 시청 구문*/
        Debug.Log("스킨 오픈을 위한 광고 시청");
        if(true/*광고 끝*/)
        {

        }
    }

    public void Back()
    {
        Debug.Log("백");

        SoundManager.Instance.PlayButtonClick();
        /*플레이어의 스킨을 현재 선택된 스킨으로 지정하는 구문*/

        //curSelectedButton.skinPrefab;

        WindowManager.Instance.Open(Windows.Main);
    }
}
