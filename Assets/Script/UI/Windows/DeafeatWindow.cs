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
        Debug.Log("ÄÁÆ¼´º");
        /*±¤°íÀç»ý*/
    }
    public void NoThanks()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("³ë¶¯Å¥");
        GameManager.GM.StartMain();
    }
}
