using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : GenericWindow
{
    public Image img;
    public bool fadeEnd;
    public void FadeOut()
    {
        base.Open();
        fadeEnd = false;
        StartCoroutine(CoFade(Color.clear, Color.black, 0.5f, false));
    }
    public void FadeIn()
    {
        base.Open();
        fadeEnd = false;
        StartCoroutine(CoFade(Color.black, Color.clear, 0.5f, true));
    }
    private IEnumerator CoFade(Color begin, Color end, float time, bool isFadeIn)
    {
        float timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;
            var lerp = Color.Lerp(begin, end, timer / time);
            img.color = lerp;
            yield return null;
        }
        fadeEnd = true;
        if(isFadeIn)
            base.Close();
    }
}
