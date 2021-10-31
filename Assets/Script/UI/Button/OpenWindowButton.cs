using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWindowButton : MonoBehaviour
{
    private bool haveNewItem;
    public Image img;
    private void Awake()
    {
        if (haveNewItem)
            img.enabled = true;
        else
            img.enabled = false;
    }
    public void OpenStorage()
    {
        if (haveNewItem)
            HaveNewItem(false);
        Debug.Log("StoragePage");
    }
    public void HaveNewItem(bool isNew)
    {
        haveNewItem = isNew;
        if(haveNewItem)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }
}
