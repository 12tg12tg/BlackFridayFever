using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skin3DCameraPrifab
{
    ae86Extra,
    aventador,
    camaro,
    delorean,
    e46Extra,
    hummer,
    rx8,
    suv,
    truck03,
    char_boy,
    char_doctor,
    char_fireman,
    char_girl,
    char_clerk,
    char_worker,
}


public class Rotate3DUI : MonoBehaviour
{
    public float rotPerSecond;

    private Dictionary<Skin3DCameraPrifab, GameObject> dic;

    public GameObject ae86Extra;
    public GameObject aventador;
    public GameObject camaro;
    public GameObject delorean;
    public GameObject e46Extra;
    public GameObject hummer;
    public GameObject rx8;
    public GameObject suv;
    public GameObject truck03;

    public GameObject char_boy;
    public GameObject char_doctor;
    public GameObject char_fireman;
    public GameObject char_girl;
    public GameObject char_clerk;
    public GameObject char_worker;

    private void Awake()
    {
        dic = new Dictionary<Skin3DCameraPrifab, GameObject>();
        dic.Add(Skin3DCameraPrifab.ae86Extra, ae86Extra);
        dic.Add(Skin3DCameraPrifab.aventador, aventador);
        dic.Add(Skin3DCameraPrifab.camaro, camaro);
        dic.Add(Skin3DCameraPrifab.delorean, delorean);
        dic.Add(Skin3DCameraPrifab.e46Extra, e46Extra);
        dic.Add(Skin3DCameraPrifab.hummer, hummer);
        dic.Add(Skin3DCameraPrifab.rx8, rx8);
        dic.Add(Skin3DCameraPrifab.suv, suv);
        dic.Add(Skin3DCameraPrifab.truck03, truck03);

        dic.Add(Skin3DCameraPrifab.char_boy, char_boy);
        dic.Add(Skin3DCameraPrifab.char_doctor, char_doctor);
        dic.Add(Skin3DCameraPrifab.char_fireman, char_fireman);
        dic.Add(Skin3DCameraPrifab.char_girl, char_girl);
        dic.Add(Skin3DCameraPrifab.char_clerk, char_clerk);
        dic.Add(Skin3DCameraPrifab.char_worker, char_worker);
    }
    private void Update()
    {
        transform.localRotation *= Quaternion.Euler(0f, rotPerSecond * Time.deltaTime, 0f);
    }

    public void SelectPrefab(Skin3DCameraPrifab prefab)
    {
        foreach (var key in dic.Keys)
        {
            if(prefab == key)
            {
                dic[key].SetActive(true);
            }
            else
            {
                dic[key].SetActive(false);
            }
        }
    }
}
