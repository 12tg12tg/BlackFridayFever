using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win1Window : GenericWindow
{
    public Text diamondTxt;
    public StageReward reward;
    public override void Open()
    {
        base.Open();

        reward.DecideReward();

        diamondTxt.text = GameManager.GM.Diamond.ToString();
        GameManager.GM.Diamond = GameManager.GM.Diamond + 100;
        GameManager.GM.AfterClear();
    }
    public override void Close()
    {

        base.Close();
    }
    public void GetX3()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("광고 영상 연결");

        /*광고끝나면*/
        GameManager.GM.Diamond = GameManager.GM.Diamond + 200;

        /*다음스테이지로*/
        GameManager.GM.StartScene();
    }
    public void Skip()
    {
        SoundManager.Instance.PlayButtonClick();

        /*다음스테이지로*/
        GameManager.GM.StartScene();
    }

    public void GoMain()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.GM.StartMain();
    }
}
