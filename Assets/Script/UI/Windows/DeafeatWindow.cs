using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeafeatWindow : GenericWindow
{
    public override void Open()
    {


        base.Open();
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
    }
    public void NoThanks()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("�붯ť");
        GameManager.GM.StartMain();
    }
}
