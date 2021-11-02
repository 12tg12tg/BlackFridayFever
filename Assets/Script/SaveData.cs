using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //��������
    public int stage;

    //��Ų
    public int skinNum;
    public int carSkinNum;

    public bool[] isSkinOpen;
    public bool[] isCarSkinOpen;

    public bool[] isSkinGet;
    public bool[] isCarSkinGet;

    public int curSkinIndex;
    public int curCarSkinIndex;


    //���� ����
    public bool isMute;
    public bool nonVibrate;

    //������
    public SaveData ()
    {

    }

}
