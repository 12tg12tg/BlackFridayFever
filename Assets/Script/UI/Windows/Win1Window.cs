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

        StartCoroutine(WaitRewardAD());

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

        StartCoroutine(WaitInterstitialAD());

    }

    private IEnumerator WaitInterstitialAD()
    {
        GameManager.GM.isInterstitialAdEnd = false;
        GoogleMobileADTest.OnClickInterstitial();
        yield return new WaitUntil(() => GameManager.GM.isInterstitialAdEnd);



        GameManager.GM.isInterstitialAdEnd = false;
        GameManager.GM.StartMain();
    }

    public IEnumerator WaitRewardAD()
    {
        GameManager.GM.isRewardAdEnd = false;
        GameManager.GM.isRewardAdRewarded = false;
        GoogleMobileADTest.OnClickReward();

        yield return new WaitUntil(() => GameManager.GM.isRewardAdEnd || GameManager.GM.isRewardAdRewarded);

        if(GameManager.GM.isRewardAdRewarded)
        {
            GameManager.GM.isRewardAdEnd = false;
            GameManager.GM.isRewardAdRewarded = false;
            GameManager.GM.Diamond = GameManager.GM.Diamond + 200;
            GameManager.GM.StartScene();
        }
        else if(GameManager.GM.isRewardAdEnd)
        {
            GameManager.GM.isRewardAdEnd = false;
            GameManager.GM.isRewardAdRewarded = false;
            GameManager.GM.StartScene();
        }
    }

}
