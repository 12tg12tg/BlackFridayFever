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
        deviceIds.Add("F8A4401AE01CD9E21F6423C24A1C0115");         //디바이스아이디 로그캣으로 확인해야함.
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);            //여기까지 출시후엔 삭제해도되는코드
        MobileAds.Initialize(initStatus => {
            RequestInterstitial();
            RequestReward();
        });
    }

    //전면광고요청
    public static void RequestInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy(); //먼저로드된 광고가 있다면 삭제
        }
        interstitial = new InterstitialAd(interstitial1Id);     //광고생성
        interstitial.OnAdLoaded += HandleOnAdLoaded;       //광고관련 델리게이트 추가
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

    //전면광고출력
    public static void OnClickInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }


    //리워드광고요청
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


    //리워드광고 출력
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
        //3배 리워드 팝업창 출력
    }
}