using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //�������� - Main
    public int openStage;
    public bool isNewSkin;
    public bool isNewCarSkin;

    //��Ų
    public int skinOpenMask;
    public int carSkinOpenMask;

    public int skinGetMask;
    public int carSkinGetMask;

    public int curSkinIndex;
    public int curCarSkinIndex;


    //���� ����
    public bool isMute;
    public bool nonVibrate;

    //���̾�
    public int diamond;

    //������������
    public int randStageIndex;
    public int[] randAiIndexs;

    //Ʃ�丮��
    public bool tutorialDone;

    //������
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
