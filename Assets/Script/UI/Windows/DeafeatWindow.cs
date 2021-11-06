using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeafeatWindow : GenericWindow
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
    public void Continue()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("��Ƽ��");
        /*�������*/
        GameManager.GM.ContinueLevel();
    }
    public void NoThanks()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("�붯ť");
        GameManager.GM.StartMain();
    }
}
