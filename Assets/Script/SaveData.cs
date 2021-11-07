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

    //랜덤스테정보
    public int randStageIndex;
    public int[] randAiIndexs;

    //튜토리얼
    public bool tutorialDone;

    //생성자
    public SaveData(int lastStageIndex, MainWindows main, StorageButtonGroup skin, StorageButtonGroup car, SoundManager sm, RandomStageInfo randStage)
    {
        openStage = lastStageIndex;
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

        randStageIndex = randStage.randStageIndex;
        randAiIndexs = randStage.randAiIndex;

        tutorialDone = GameManager.GM.tutorialDone;
    }

}
