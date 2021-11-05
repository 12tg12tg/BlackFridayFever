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

    public Text diamondTxt;

    public int finalStage = 15;

    //public int invokeStageNum;
    public int lastOpenedStage;

    private void Awake()
    {
        /*저장공간에서 스테이지 정보 불러오기*/

        //현재 스테이지 넘버 설정 (임시 : 1)
        lastOpenedStage = 12;
        //invokeStageNum = lastOpenedStage;

        //버튼 활성화여부 결정
        //DetermineButtonOnOff();

        //중앙 텍스트 구성
        //MakeText();
    }

    public void Init(int lastOpenStage, bool isNewSkin, bool isNewCarSkin)
    {
        this.lastOpenedStage = lastOpenStage;
        characterSkin.HaveNewItem(isNewSkin);
        carSkin.HaveNewItem(isNewCarSkin);
    }

    public override void Open()
    {
        base.Open();
        diamondTxt.text = GameManager.GM.Diamond.ToString();
    }

    private void Update()
    {
        if(mt.Swipe.x > 0)
        {
            GameManager.GM.SaveData();
            LoadStageScene();
        }
    }


    public void LoadStageScene()
    {
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene($"Stage{lastOpenedStage}", LoadSceneMode.Additive);
        GameManager.isStage = true;
    }

    public void BackStage()
    {
        //invokeStageNum--;
        //DetermineButtonOnOff();
        //MakeText();
        //SoundManager.Instance.PlayButtonClick();
    }

    public void NextStage()
    {
        //invokeStageNum++;
        //DetermineButtonOnOff();
        //MakeText();
        //SoundManager.Instance.PlayButtonClick();
    }

    public void BackFiveStage()
    {
        //invokeStageNum -= 5;
        //DetermineButtonOnOff();
        //MakeText();
        //SoundManager.Instance.PlayButtonClick();
    }

    public void NextFiveStage()
    {
        //invokeStageNum += 5;
        //DetermineButtonOnOff();
        //MakeText();
        //SoundManager.Instance.PlayButtonClick();
    }

    public void DetermineButtonOnOff()
    {
        //prev1.interactable = invokeStageNum >= 2;

        //prev5.interactable = invokeStageNum >= 6;

        //next1.interactable = invokeStageNum <= lastOpenedStage - 1;

        //next5.interactable = invokeStageNum <= lastOpenedStage - 5;
    }

    public void MakeText()
    {
        //text.text = $"LEVEL {invokeStageNum.ToString("D2")}";
    }


    public OpenWindowButton characterSkin;
    public OpenWindowButton carSkin;
    public void OpenSkinStorage()
    {
        if (characterSkin.haveNewItem)
            characterSkin.HaveNewItem(false);
        WindowManager.Instance.Open(Windows.SkinStorage);
        SoundManager.Instance.PlayButtonClick();
    }
    public void OpenCarSkinStorage()
    {
        if (carSkin.haveNewItem)
            carSkin.HaveNewItem(false);
        WindowManager.Instance.Open(Windows.CarSkinStorage);
        SoundManager.Instance.PlayButtonClick();
    }

    public void SetMute(bool isMute)
    {
        SoundManager.Instance.SetMute(isMute);
    }
    public void SetVibrate(bool noVibrate)
    {
        SoundManager.Instance.SetVibrate(noVibrate);
    }
}
