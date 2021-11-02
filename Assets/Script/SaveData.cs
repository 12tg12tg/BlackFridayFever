using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //스테이지
    public int stage;

    //스킨
    public int skinNum;
    public int carSkinNum;

    public bool[] isSkinOpen;
    public bool[] isCarSkinOpen;

    public bool[] isSkinGet;
    public bool[] isCarSkinGet;

    public int curSkinIndex;
    public int curCarSkinIndex;


    //사운드 정보
    public bool isMute;
    public bool nonVibrate;

    //생성자
    public SaveData ()
    {

    }

}
