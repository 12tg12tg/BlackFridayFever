using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTransparents : MonoBehaviour
{
    public MeshRenderer[] aboutShops;
    public MeshRenderer[] buildingLogo;
    public Material shopOriginal;
    public Material shopTransparent;
    public Material logoOriginal;
    public Material logoTransparent;
    void Update()
    {
        var playerPos = GameManager.GM.player.transform.position;
        var playerViewport = Camera.main.WorldToViewportPoint(playerPos);
        Ray ray = Camera.main.ViewportPointToRay(playerViewport);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50))
        {
            if (hit.transform.gameObject == gameObject)
            {
                MakeTransparent();
            }
            else
                TransparentOff();
        }
    }
    void MakeTransparent()
    {
        foreach (var item in aboutShops)
        {
            item.material = shopTransparent;
        }
        foreach (var item in buildingLogo)
        {
            item.material = logoTransparent;
        }
    }
    void TransparentOff()
    {
        foreach (var item in aboutShops)
        {
            item.material = shopOriginal;
        }
        foreach (var item in buildingLogo)
        {
            item.material = logoOriginal;
        }
    }
}
