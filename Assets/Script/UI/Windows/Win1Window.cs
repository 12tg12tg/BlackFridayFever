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
        Debug.Log("���� ���� ����");

        /*��������*/
        GameManager.GM.Diamond = GameManager.GM.Diamond + 300;

        GameManager.GM.main.lastOpenedStage++;
        GameManager.GM.SaveData();
        /*��������������*/
        GameManager.GM.GoNextLevel();
    }
    public void Skip()
    {
        SoundManager.Instance.PlayButtonClick();

        GameManager.GM.Diamond = GameManager.GM.Diamond + 100;

        GameManager.GM.main.lastOpenedStage++;
        GameManager.GM.SaveData();

        GameManager.GM.GoNextLevel();
        //Debug.Log("���� ����������");
    }
}
