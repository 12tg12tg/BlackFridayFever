using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Windows
{
    Win,
    RewardPopUp,
    Defeated,
    Main,
}

public class WindowManager : MonoBehaviour
{
    public GenericWindow[] windows;
    public Windows defaultWindowId; //ó����������
    private Windows currentWindowId; //���翭��������
    private Windows additiveWindowId; //�߰��ο���������
    public bool isAddtiveOpen;

    private static WindowManager instance; //static �ʵ�
    public static WindowManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
    }
    public void Init()
    {
        for (int i = 0; i < windows.Length; ++i)
        {
            windows[i].gameObject.SetActive(false);
        }
        currentWindowId = defaultWindowId;
        windows[(int)defaultWindowId].Open();
    }
    public GenericWindow GetWindow(Windows id)
    {
        return windows[(int)id];
    }
    public GenericWindow Open(Windows id)
    {
        windows[(int)currentWindowId].Close();
        currentWindowId = id;
        windows[(int)currentWindowId].Open();
        return windows[(int)currentWindowId];
    }
    public GenericWindow PopupWindow(Windows id)
    {
        isAddtiveOpen = true;
        additiveWindowId = id;
        windows[(int)additiveWindowId].Open();
        return windows[(int)additiveWindowId];
    }
}
