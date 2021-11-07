using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageReward : MonoBehaviour
{
    public Dictionary<int, List<StorageButton>> rewardDic;

    public StorageButton car_aventador;
    public StorageButton car_ae86;
    public StorageButton car_camaro;
    public StorageButton car_delorean;
    public StorageButton car_e46;
    public StorageButton car_hummer;
    public StorageButton car_rx8;
    public StorageButton car_suv;
    public StorageButton car_truck03;

    public StorageButton skin_boy;
    public StorageButton skin_girl;
    public StorageButton skin_doctor;
    public StorageButton skin_fireman;
    public StorageButton skin_clerk;
    public StorageButton skin_worker;

    public void Awake()
    {
        rewardDic = new Dictionary<int, List<StorageButton>>();
        rewardDic.Add(5, new List<StorageButton>());
        rewardDic.Add(10, new List<StorageButton>());
        rewardDic.Add(15, new List<StorageButton>());
        rewardDic.Add(20, new List<StorageButton>());
        rewardDic.Add(25, new List<StorageButton>());
        rewardDic.Add(30, new List<StorageButton>());
        rewardDic.Add(35, new List<StorageButton>());
        rewardDic.Add(40, new List<StorageButton>());
        rewardDic.Add(45, new List<StorageButton>());
        rewardDic.Add(50, new List<StorageButton>());
        rewardDic.Add(60, new List<StorageButton>());

        rewardDic[5].Add(car_suv);
        rewardDic[10].Add(car_camaro);
        rewardDic[15].Add(car_e46);
        rewardDic[20].Add(car_delorean);
        rewardDic[25].Add(car_ae86);
        rewardDic[30].Add(car_hummer);
        rewardDic[35].Add(car_aventador);
        rewardDic[40].Add(car_rx8);
        rewardDic[45].Add(car_truck03);

        rewardDic[10].Add(skin_boy);
        rewardDic[20].Add(skin_girl);
        rewardDic[30].Add(skin_worker);
        rewardDic[40].Add(skin_fireman);
        rewardDic[50].Add(skin_doctor);
        rewardDic[60].Add(skin_clerk);
    }

    public void DecideReward()
    {
        int curClearStage = GameManager.GM.lastOpenedStage; //클리어직후로서 아직증가하지않음.

        List<StorageButton> curSkinButton;

        if (rewardDic.TryGetValue(curClearStage, out curSkinButton))
        {
            StartCoroutine(CoRewardPopUp(curSkinButton));
        }
    }

    public IEnumerator CoRewardPopUp(List<StorageButton> curSkinButton)
    {
        for (int i = 0; i < curSkinButton.Count; i++)
        {
            if (!curSkinButton[i].isOpened)
            {
                curSkinButton[i].isOpened = true;
                curSkinButton[i].buttonGroup.remainSkin--;
                curSkinButton[i].buttonGroup.MakeSaveMask();
                curSkinButton[i].buttonGroup.rinkButton.HaveNewItem(true);
                GameManager.GM.SaveData();

                /*리워드 프리펩을 제2의 카메라 위치로!*/
                curSkinButton[i].buttonGroup.UI3D.SelectPrefab(curSkinButton[i].skinEnum);
                var genericWindow = WindowManager.Instance.PopupWindow(Windows.RewardPopUp);

                yield return new WaitUntil(() => !genericWindow.gameObject.activeInHierarchy);
                Debug.Log("창이닫힘");
            }
        }
    }
}
