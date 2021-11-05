using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject player;
    public GameObject sampleCar;

    public Transform playerSkinParent;
    public Transform carParent;

    public GameObject defalutPlayerSkin;
    public GameObject defalutCarSkin;

    public GameObject curSkin;
    private GameObject curCar;

    public GameObject target1;
    public GameObject target2;

    private void Start()
    {
        SetSkin();
    }

    public void SetSkin()
    {
        var skin = GameManager.GM.skin.currentSkin;
        var car = GameManager.GM.car.currentSkin;
        if(skin != null || car != null)
        {
            target1.SetActive(false);
            target2.SetActive(false);

            player.SetActive(true);
            sampleCar.SetActive(true);

            if (curSkin != null)
                Destroy(curSkin);
            if (curCar != null)
                Destroy(curCar);

            if (skin != null)
            {
                curSkin = Instantiate(skin, playerSkinParent);
                defalutPlayerSkin.SetActive(false);
                var animator = curSkin.GetComponentInChildren<Animator>();
                animator.SetTrigger("Lean");
            }
            else
            {
                defalutPlayerSkin.SetActive(true);
                var animator = defalutPlayerSkin.GetComponentInChildren<Animator>();
                animator.SetTrigger("Lean");
            }

            if(car != null)
            {
                curCar = Instantiate(car, carParent);
                defalutCarSkin.SetActive(false);
            }
            else
            {
                defalutCarSkin.SetActive(true);
            }
        }
        else
        {
            target1.SetActive(true);
            target2.SetActive(true);

            player.SetActive(false);
            sampleCar.SetActive(false);
        }
    }
}
