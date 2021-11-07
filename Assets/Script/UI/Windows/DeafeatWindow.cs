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

        StartCoroutine(WaitRewardAD());
    }
    public void NoThanks()
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

        if (GameManager.GM.isRewardAdRewarded)
        {
            GameManager.GM.isRewardAdEnd = false;
            GameManager.GM.isRewardAdRewarded = false;

            GameManager.GM.ContinueLevel();
        }
        else if (GameManager.GM.isRewardAdEnd)
        {
            GameManager.GM.isRewardAdEnd = false;
            GameManager.GM.isRewardAdRewarded = false;

            GameManager.GM.ContinueLevel();
        }
    }
}
