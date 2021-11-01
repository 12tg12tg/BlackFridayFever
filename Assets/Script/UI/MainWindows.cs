using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainWindows : GenericWindow
{
    public MultiTouch mt;
    public Button prev1;
    public Button prev5;
    public Button next1;
    public Button next5;
    public Text text;

    public int finalStage = 15;

    public int invokeStageNum;
    public int lastOpenedStage;

    private void Awake()
    {
        /*����������� �������� ���� �ҷ�����*/

        //���� �������� �ѹ� ���� (�ӽ� : 5)
        lastOpenedStage = 5;
        invokeStageNum = lastOpenedStage;

        //��ư Ȱ��ȭ���� ����
        DetermineButtonOnOff();

        //�߾� �ؽ�Ʈ ����
        MakeText();
    }

    private void Update()
    {
        if(mt.Swipe.x > 0)
        {
            LoadStageScene();
        }
    }


    public void LoadStageScene()
    {
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene(invokeStageNum + 1, LoadSceneMode.Additive);
        GameManager.isStage = true;
    }

    public void BackStage()
    {
        invokeStageNum--;
        DetermineButtonOnOff();
        MakeText();
    }

    public void NextStage()
    {
        invokeStageNum++;
        DetermineButtonOnOff();
        MakeText();
    }

    public void BackFiveStage()
    {
        invokeStageNum -= 5;
        DetermineButtonOnOff();
        MakeText();
    }

    public void NextFiveStage()
    {
        invokeStageNum += 5;
        DetermineButtonOnOff();
        MakeText();
    }

    public void DetermineButtonOnOff()
    {
        prev1.interactable = invokeStageNum >= 2;

        prev5.interactable = invokeStageNum >= 6;

        next1.interactable = invokeStageNum <= lastOpenedStage - 1;

        next5.interactable = invokeStageNum <= lastOpenedStage - 5;
    }

    public void MakeText()
    {
        text.text = $"LEVEL {invokeStageNum.ToString("D2")}";
    }


    public OpenWindowButton characterSkin;
    public OpenWindowButton carSkin;
    public void OpenSkinStorage()
    {
        if (characterSkin.haveNewItem)
            characterSkin.HaveNewItem(false);
        WindowManager.Instance.Open(Windows.SkinStorage);
    }
    public void OpenCarSkinStorage()
    {
        if (carSkin.haveNewItem)
            carSkin.HaveNewItem(false);
        WindowManager.Instance.Open(Windows.CarSkinStorage);
    }
}
