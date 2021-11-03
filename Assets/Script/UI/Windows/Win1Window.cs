using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win1Window : GenericWindow
{
    public Text diamondTxt;
    public override void Open()
    {
        base.Open();
        diamondTxt.text = GameManager.GM.Diamond.ToString();
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
        GameManager.GM.Diamond = GameManager.GM.Diamond + 300;
        GameManager.GM.SaveData();
        /*다음스테이지로*/
    }
    public void Skip()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.GM.GoNextLevel();
        //Debug.Log("다음 스테이지로");

        GameManager.GM.Diamond = GameManager.GM.Diamond + 100;

        GameManager.GM.SaveData();
    }
}
