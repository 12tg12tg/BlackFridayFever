using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpButton : MonoBehaviour
{
    public RectTransform popButton;
    public float cycle;
    private float timer;
    private bool isSizeDown;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > cycle)
        {
            timer = 0f;
            isSizeDown = !isSizeDown;
        }


        float lerp;
        if (!isSizeDown)
        {
            var ratio = timer / cycle;
            lerp = Mathf.Lerp(1.0f, 1.2f, ratio);
        }
        else
        {
            var ratio = timer / cycle;
            lerp = Mathf.Lerp(1.2f, 1.0f, ratio);
        }
        popButton.localScale = new Vector3(lerp, lerp, lerp);
    }
}
