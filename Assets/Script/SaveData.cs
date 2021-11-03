using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //스테이지 - Main
    public int openStage;
    public bool isNewSkin;
    public bool isNewCarSkin;

    //스킨
    public int skinOpenMask;
    public int carSkinOpenMask;

    public int skinGetMask;
    public int carSkinGetMask;

    public int curSkinIndex;
    public int curCarSkinIndex;


    //사운드 정보
    public bool isMute;
    public bool nonVibrate;

    //다이아
    public int diamond;

    //생성자
    public SaveData(MainWindows main, StorageButtonGroup skin, StorageButtonGroup car, SoundManager sm)
    {
        openStage = main.lastOpenedStage;
        isNewSkin = main.characterSkin.haveNewItem;
        isNewCarSkin = main.carSkin.haveNewItem;

        skinOpenMask = skin.openMask;
        carSkinOpenMask = car.openMask;

        skinGetMask = skin.buyMask;
        carSkinGetMask = car.buyMask;

        curSkinIndex = skin.curSelectedButton;
        curCarSkinIndex = car.curSelectedButton;

        isMute = sm.isMute;
        nonVibrate = sm.noVibrate;

        diamond = GameManager.GM.Diamond;
    }

}
