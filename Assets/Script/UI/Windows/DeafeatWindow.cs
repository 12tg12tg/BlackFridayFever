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
        Debug.Log("컨티뉴");
        /*광고재생*/
    }
    public void NoThanks()
    {
        Debug.Log("노땡큐");
        /*메인화면으로*/
    }
}
