using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageButtonGroup : GenericWindow
{
    public Text diamondTxt;

    public StorageButton[] buttons;
    public Button AD_Link;
    public Button buy500;

    public int curSelectedButton;           //★
    public StorageButton curFocusButton;

    public int remainSkin;

    public int openMask;
    public int buyMask;

    public GameObject currentSkin
    {
        get
        {
            if(curSelectedButton == -1)
            {
                return null;
            }
            else
            {
                return buttons[curSelectedButton].skinPrefab;
            }
        }
    }

    public void Init(int openMask, int buyMask, int currentSelected)
    {
        /*로드구문*/
        for (int i = 0; i < buttons.Length; i++)
        {
            if((openMask >> i & 1) == 1)
            {
                buttons[i].isOpened = true;
            }
            else
            {
                buttons[i].isOpened = false;
                remainSkin++;
            }

            if ((buyMask >> i & 1) == 1)
            {
                buttons[i].isBuy = true;
            }
            else
            {
                buttons[i].isBuy = false;
            }

            if(currentSelected == i)
            {
                buttons[i].isSelectedSkin = true;
            }
            else
            {
                buttons[i].isSelectedSkin = false;
            }
        }

        this.openMask = openMask;
        this.buyMask = buyMask;
        this.curSelectedButton = currentSelected;
        ADButtonOnOFF();
        buy500.interactable = false;
    }

    public override void Open()
    {
        base.Open();
        diamondTxt.text = GameManager.GM.Diamond.ToString();
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
                    curSelectedButton = i;
                }
                else
                {
                    curSelectedButton = -1;
                }
            }
            else
            {
                buttons[i].isSelectedSkin = false;
            }
        }
        curFocusButton = null;
        GameManager.GM.SaveData();
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
            if (curFocusButton.isOpened && !curFocusButton.isBuy)
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
        if (GameManager.GM.Diamond >= 500)
        {
            GameManager.GM.Diamond = GameManager.GM.Diamond - 300;
            diamondTxt.text = GameManager.GM.Diamond.ToString();
            curFocusButton.isBuy = true;

            /*리워드 프리펩을 제2의 카메라 위치로!*/
            WindowManager.Instance.PopupWindow(Windows.RewardPopUp);
        }
        else
        {
            //금액이 모자라다면 아무일도 일어나지 않는다. (혹은, 보석 상자 진동)

        }

        MakeSaveMask();
        GameManager.GM.SaveData();
    }

    public void WatchAdForSkin()
    {
        SoundManager.Instance.PlayButtonClick();

        /*광고 시청 구문*/
        Debug.Log("스킨 오픈을 위한 광고 시청");
        if(true/*광고 끝*/)
        {
            var rand = Random.Range(0, remainSkin);
            int count = -1;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (!buttons[i].isOpened)
                {
                    count++;
                    if(count == rand)
                    {
                        buttons[i].isOpened = true;
                        --remainSkin;
                        break;
                    }
                }
            }
        }
        MakeSaveMask();
        GameManager.GM.SaveData();
    }

    public void Back()
    {
        Debug.Log("백");

        SoundManager.Instance.PlayButtonClick();
        /*플레이어의 스킨을 현재 선택된 스킨으로 지정하는 구문*/

        //curSelectedButton.skinPrefab;

        WindowManager.Instance.Open(Windows.Main);
        GameManager.GM.SaveData();
    }

    public void MakeSaveMask()
    {
        openMask = 0;
        buyMask = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].isOpened)
            {
                openMask += 1 << i;

                if(buttons[i].isBuy)
                {
                    buyMask += 1 << i;
                }
            }
        }
    }
}
