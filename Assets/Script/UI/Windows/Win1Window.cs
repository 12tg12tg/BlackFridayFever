using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win1Window : GenericWindow
{
    public override void Open()
    {


        base.Open();
    }
    public override void Close()
    {


        base.Close();
    }
    public void GetX3()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("���� ���� ����");
    }
    public void Skip()
    {
        SoundManager.Instance.PlayButtonClick();
        GameManager.GM.GoNextLevel();
        Debug.Log("���� ����������");
    }
}
