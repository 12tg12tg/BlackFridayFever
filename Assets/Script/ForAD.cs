using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAD : MonoBehaviour
{
    public MultiTouch mt;
    public Collider Door;
    public GameObject Bar;
    public float timer;
    Quaternion original;
    private void Start()
    {
        mt = Camera.main.transform.GetComponent<MultiTouch>();
        original = Bar.transform.rotation;
    }
    void Update()
    {
        if(mt.Swipe.y > 0)
        {
            Debug.Log("광고용 문 열림");
            StartCoroutine(CoGateOpen(2f));
        }
    }
    public IEnumerator CoGateOpen(float time)
    {

        while (timer < time)
        {
            timer += Time.deltaTime;
            float degree = Mathf.Lerp(0f, 90f, timer / time);
            
            Bar.transform.rotation = original * Quaternion.Euler(degree, 0f, 0f);
            yield return null;
        }
        Door.enabled = false;
    }
}
