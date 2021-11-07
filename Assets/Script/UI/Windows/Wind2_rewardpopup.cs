using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind2_rewardpopup : GenericWindow
{
    public override void Open()
    {
        base.Open();
        SoundManager.Instance.PlayRewardGet();
    }
    public override void Close()
    {


        base.Close();
    }
    public void OK()
    {
        SoundManager.Instance.PlayButtonClick();
        Debug.Log("»õ ½ºÅ² È¹µæ");
        Close();
    }
}
