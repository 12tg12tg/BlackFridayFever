using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public static class GoogleMobileADTest
{
    public static readonly string interstitial1Id = "ca-app-pub-1195551850458243/4490348779";
    public static readonly string reward1Id = "ca-app-pub-1195551850458243/5611858753";

    private static InterstitialAd interstitial;
    private static RewardedAd rewardedAd;

    public static void Init()
    {
        List<string> deviceIds = new List<string>();
        deviceIds.Add("F8A4401AE01CD9E21F6423C24A1C0115");         //����̽����̵� �α�Ĺ���� Ȯ���ؾ���.
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);            //������� ����Ŀ� �����ص��Ǵ��ڵ�
        MobileAds.Initialize(initStatus => {
            RequestInterstitial();
            RequestReward();
        });
    }

    //���鱤���û
    public static void RequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy(); //�����ε�� ���� �ִٸ� ����
        }
        interstitial = new InterstitialAd(interstitial1Id);     //�������
        interstitial.OnAdLoaded += HandleOnAdLoaded;       //������� ��������Ʈ �߰�
        interstitial.OnAdOpening += HandleOnAdOpened;
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdFailedToLoad += OnAdFailedToLoad;

        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public static void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLoaded event received");
    }

    public static void HandleOnAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
    }

    public static void HandleOnAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
    }

    public static void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    //���鱤�����
    public static void OnClickInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }


    //�����層���û
    public static void RequestReward()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        rewardedAd = new RewardedAd(reward1Id);
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdClosed += HandleRewardedOnAdClosed;
        rewardedAd.OnAdFailedToLoad += OnAdFailedToLoad;

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    public static void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public static void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdOpening event received");
    }
    public static void HandleRewardedOnAdClosed(object sender, EventArgs args)
    {
        RequestReward();
    }


    //�����層�� ���
    public static void OnClickReward()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    public static void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        //3�� ������ �˾�â ���
    }
}