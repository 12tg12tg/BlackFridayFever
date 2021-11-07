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
        Debug.Log("���� ���� ����");

        /*��������*/
        GameManager.GM.Diamond = GameManager.GM.Diamond + 200;

        /*��������������*/
        GameManager.GM.StartScene();
    }
    public void Skip()
    {
        SoundManager.Instance.PlayButtonClick();

        /*��������������*/
        GameManager.GM.StartScene();
    }

    public void GoMain()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.GM.StartMain();
    }
}
