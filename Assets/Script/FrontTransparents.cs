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

    private void Awake()
    {
        if (aboutShops.Length != 0)
            return;
        var meshs = GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] shops = new MeshRenderer[meshs.Length - 2];
        int index = 0;
        for (int i = 0; i < meshs.Length; i++)
        {
            if (meshs[i] == buildingLogo[0] || meshs[i] == buildingLogo[1])
            {

            }
            else
            {
                shops[index++] = meshs[i];
            }
        }
        aboutShops = shops;
    }

    void Update()
    {
        var playerPos = GameManager.GM.player.transform.position;
        if (playerPos.z > transform.position.z + 2f)
        {
            TurnOffMeshs();
        }
        else
        {
            TurnOnMeshs();

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
    }

    void TurnOffMeshs()
    {
        foreach (var item in aboutShops)
        {
            item.enabled = false;
        }
        foreach (var item in buildingLogo)
        {
            item.enabled = false;
        }
    }

    void TurnOnMeshs()
    {
        foreach (var item in aboutShops)
        {
            item.enabled = true;
        }
        foreach (var item in buildingLogo)
        {
            item.enabled = true;
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
