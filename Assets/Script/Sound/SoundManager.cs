using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public bool isMute;
    public bool noVibrate;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    public AudioSource bgmPlayer;
    public AudioSource takePlayer;
    public AudioSource playerPlayer;
    public AudioSource buttonPlayer;
    public AudioSource ragdollPlayer;

    public AudioClip gameBgm;
    public AudioClip crushWin;
    public AudioClip crushLose;
    public AudioClip buttonClick;
    public AudioClip takeMoney;
    public AudioClip takeItem;
    public AudioClip loadTruck;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip rewardGetSound;

    public ToggleScript soundToggle;
    public ToggleScript vibrateToggle;

    private void Awake()
    {
        instance = this;
    }

    public void SetMute(bool isMute)
    {
        this.isMute = isMute;
        soundToggle.ChangeImage(isMute);
        if (isMute)
            Debug.Log("음소거");
        else
            Debug.Log("소리On");
    }
    public void SetVibrate(bool noVibrate)
    {
        this.noVibrate = noVibrate;
        vibrateToggle.ChangeImage(noVibrate);
        if (noVibrate)
            Debug.Log("진동제거");
        else
            Debug.Log("진동On");
    }
    public void PlayGameBGM()
    {
        if(!isMute)
            bgmPlayer.Play();
    }
    public void StopGameBGM()
    {
        if (!isMute)
            bgmPlayer.Stop();
    }
    public void PlayCrushWin()
    {
        if (!isMute)
            playerPlayer.PlayOneShot(crushWin);
    }
    public void PlayCrushLose()
    {
        if (!isMute)
            playerPlayer.PlayOneShot(crushLose);
    }
    public void PlayButtonClick()
    {
        if (!isMute)
            buttonPlayer.PlayOneShot(buttonClick);
    }
    public void PlayTakeMoney()
    {
        if (!isMute)
            takePlayer.PlayOneShot(takeMoney);
    }
    public void PlayTakeItem()
    {
        if (!isMute)
            takePlayer.PlayOneShot(takeItem);
    }
    public void PlayLoadTruck()
    {
        if (!isMute)
            takePlayer.PlayOneShot(loadTruck);
    }
    public void PlayWinBgm()
    {
        if (!isMute)
            bgmPlayer.PlayOneShot(winSound);
    }
    public void PlayLoseBgm()
    {
        if (!isMute)
            bgmPlayer.PlayOneShot(loseSound);
    }
    public void PlayAiCrush()
    {
        if (!isMute)
            if (!ragdollPlayer.isPlaying)
                ragdollPlayer.Play();
    }
    public void PlayRewardGet()
    {
        if (!isMute)
            buttonPlayer.PlayOneShot(rewardGetSound);
    }
    public void Vibrate()
    {
        if (!noVibrate)
        {
            Handheld.Vibrate();
            Debug.Log("진동울림");
        }
    }
}
