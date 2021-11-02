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
        /*static ���� Save ����ü���� �ݾ� Ȯ�� �� ����*/
        if (true/*�ݾ� �� ����*/)
        {
            //�ݾ��� ����ϴٸ� �����Ͽ� ��Ų���
            curFocusButton.isOpened = true;
        }
        else
        {
            //�ݾ��� ���ڶ�ٸ� �ƹ��ϵ� �Ͼ�� �ʴ´�. (Ȥ��, ���� ���� ����)

        }
    }

    public void WatchAdForSkin()
    {
        SoundManager.Instance.PlayButtonClick();

        /*���� ��û ����*/
        Debug.Log("��Ų ������ ���� ���� ��û");
        if(true/*���� ��*/)
        {

        }
    }

    public void Back()
    {
        Debug.Log("��");

        SoundManager.Instance.PlayButtonClick();
        /*�÷��̾��� ��Ų�� ���� ���õ� ��Ų���� �����ϴ� ����*/

        //curSelectedButton.skinPrefab;

        WindowManager.Instance.Open(Windows.Main);
    }
}
