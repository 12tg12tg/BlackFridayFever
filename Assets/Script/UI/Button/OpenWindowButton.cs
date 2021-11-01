using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWindowButton : MonoBehaviour
{
    public bool haveNewItem;
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
