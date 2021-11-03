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

    //������
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
