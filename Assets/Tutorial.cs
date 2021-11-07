using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject Image1;
    public GameObject Image2;

    public bool isDone;

    public void Open1()
    {
        gameObject.SetActive(true);
        Image1.SetActive(true);
    }
    public void Open2()
    {
        Image1.SetActive(false);
        Image2.SetActive(true);
    }
    public void CloseTutorial()
    {
        Image2.SetActive(false);
        gameObject.SetActive(false);
        isDone = true;
    }
}
