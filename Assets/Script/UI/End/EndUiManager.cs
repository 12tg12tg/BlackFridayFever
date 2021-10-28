using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndScnenWindows
{
    Win1,
    Win2,
    Defeated
}

public class EndUiManager : MonoBehaviour
{
    public GenericWindow[] windows;
    public EndScnenWindows defaultWindowId; //처음열윈도우
    private EndScnenWindows currentWindowId; //현재열린윈도우

    private static EndUiManager instance; //static 필드
    public static EndUiManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
        Init();
    }
    private void Init()
    {
        for (int i = 0; i < windows.Length; ++i)
        {
            windows[i].gameObject.SetActive(false);
        }
        currentWindowId = defaultWindowId;
        windows[(int)defaultWindowId].Open();
    }
    public GenericWindow GetWindow(EndScnenWindows id)
    {
        return windows[(int)id];
    }
    public GenericWindow Open(EndScnenWindows id)
    {
        windows[(int)currentWindowId].Close();
        currentWindowId = id;
        windows[(int)currentWindowId].Open();
        return windows[(int)currentWindowId];
    }
}
