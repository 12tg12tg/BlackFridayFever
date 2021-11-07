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

    public int curSelectedButton;           //��
    public StorageButton curFocusButton;

    public int remainSkin;

    public int openMask;
    public int buyMask;

    public MainScript main;
    public OpenWindowButton rinkButton;

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
        /*�ε屸��*/
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

    public Rotate3DUI UI3D;
    public void BuySkin()
    {
        SoundManager.Instance.PlayButtonClick();
        /*static ���� Save ����ü���� �ݾ� Ȯ�� �� ����*/
        if (GameManager.GM.Diamond >= 500)
        {
            GameManager.GM.Diamond = GameManager.GM.Diamond - 500;
            diamondTxt.text = GameManager.GM.Diamond.ToString();
            curFocusButton.isBuy = true;

        }
        else
        {
            //�ݾ��� ���ڶ�ٸ� �ƹ��ϵ� �Ͼ�� �ʴ´�. (Ȥ��, ���� ���� ����)

        }

        MakeSaveMask();
        GameManager.GM.SaveData();
    }

    public void WatchAdForSkin()
    {
        SoundManager.Instance.PlayButtonClick();

        StartCoroutine(WaitRewardAD());


    }

    public void Back()
    {
        Debug.Log("��");

        SoundManager.Instance.PlayButtonClick();
        /*�÷��̾��� ��Ų�� ���� ���õ� ��Ų���� �����ϴ� ����*/

        //curSelectedButton.skinPrefab;

        WindowManager.Instance.Open(Windows.Main);
        GameManager.GM.SaveData();

        if(main == null)
        {
            var mainCenter = GameObject.Find("mainCenter");
            main = mainCenter?.GetComponent<MainScript>();
        }
        main?.SetSkin();
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

    public IEnumerator WaitRewardAD()
    {
        GameManager.GM.isRewardAdEnd = false;
        GameManager.GM.isRewardAdRewarded = false;
        GoogleMobileADTest.OnClickReward();

        yield return new WaitUntil(() => GameManager.GM.isRewardAdEnd || GameManager.GM.isRewardAdRewarded);

        if (GameManager.GM.isRewardAdRewarded)
        {
            GameManager.GM.isRewardAdEnd = false;
            GameManager.GM.isRewardAdRewarded = false;

            var rand = Random.Range(0, remainSkin);
            int count = -1;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (!buttons[i].isOpened)
                {
                    count++;
                    if (count == rand)
                    {
                        buttons[i].isOpened = true;
                        --remainSkin;

                        /*������ �������� ��2�� ī�޶� ��ġ��!*/
                        UI3D.SelectPrefab(buttons[i].skinEnum);
                        WindowManager.Instance.PopupWindow(Windows.RewardPopUp);
                        break;
                    }
                }
            }
            MakeSaveMask();
            GameManager.GM.SaveData();
        }
        else if (GameManager.GM.isRewardAdEnd)
        {
            GameManager.GM.isRewardAdEnd = false;
            GameManager.GM.isRewardAdRewarded = false;

        }
    }
}
