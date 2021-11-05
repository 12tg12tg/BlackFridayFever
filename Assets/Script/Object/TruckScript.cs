using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    [Header("(Inspector ���� : 1��)")]
    public int currentScore;
    public Transform dokingSpot;
    public Transform[] camPosForSave;
    [Header("(Inspector ����) BodyMesh�� ���� ���� �ش��ϴ� ������ MeshRenderer ����)")]
    public Color bodyColor;

    [Header("(��Ų ���� : �˾Ƽ� ��")]
    public Transform skin;
    public GameObject defaltSkin;

    private void Awake()
    {

    }

    public void SkinInit(GameObject skinPrefab)
    {
        if (skinPrefab != null)
        {
            Instantiate(skinPrefab, skin);
            var meshs = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshs.Length; i++)
            {
                if (meshs[i].tag == "Skin")
                {
                    bodyColor = meshs[i].material.color;
                    break;
                }
            }
        }
        else
        {
            Instantiate(defaltSkin, skin);
            var meshs = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshs.Length; i++)
            {
                if (meshs[i].tag == "Skin")
                {
                    var r = Random.Range(0f, 1f);
                    var g = Random.Range(0f, 1f);
                    var b = Random.Range(0f, 1f);
                    Color rndColor = new Color(r, g, b);
                    meshs[i].material.color = rndColor;
                    bodyColor = rndColor;
                    break;
                }
            }
        }
    }

    public void SavePurchased(int score)
    {
        currentScore += score;
        //Debug.Log("Save!");
    }
}
