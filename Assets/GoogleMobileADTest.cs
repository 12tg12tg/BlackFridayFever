using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class GoogleMobileADTest : MonoBehaviour
{
    public Button interstitialButton;
    public Button rewardButton;
    public Text reward;

    public static readonly string interstitial1Id = "ca-app-pub-1195551850458243/4490348779";
    public static readonly string reward1Id = "ca-app-pub-1195551850458243/5611858753";

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    private void Start()
    {
        interstitialButton.interactable = false;
        rewardButton.interactable = false;
    }


    public void OnClickInit()
    {
        List<string> deviceIds = new List<string>();
        deviceIds.Add("F8A4401AE01CD9E21F6423C24A1C0115");         //����̽����̵� �α�Ĺ���� Ȯ���ؾ���.
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);            //������� ����Ŀ� �����ص��Ǵ��ڵ�
        MobileAds.Initialize(initStatus => { });
    }

    //���鱤���û
    public void OnClickRequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy(); //�����ε�� ���� �ִٸ� ����
        }
        interstitial = new InterstitialAd(interstitial1Id);     //�������
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;       //������� ��������Ʈ �߰�
        this.interstitial.OnAdOpening += HandleOnAdOpened;

        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        interstitialButton.interactable = true;
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        interstitialButton.interactable = false;
    }


    //���鱤�����
    public void OnClickInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }


    //�����層���û
    public void OnClickRequestReward()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        this.rewardedAd = new RewardedAd(reward1Id);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
        rewardButton.interactable = true;
        reward.text = "������ ���� ���";
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
        rewardButton.interactable = false;
    }


    //�����層�� ���
    public void OnClickReward()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        reward.text = "received for " + amount.ToString() + " " + type;
    }
}