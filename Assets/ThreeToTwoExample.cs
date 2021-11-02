using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeToTwoExample : MonoBehaviour
{
    public Transform black3;
    public Transform red3;
    public Transform player3;

    public Vector2 black2;
    public Vector2 red2;
    public Vector2 player2;

    public RectTransform indicateRed;
    public RectTransform indicateBlk;

    private void Update()
    {
        black2 = Camera.main.WorldToScreenPoint(black3.position);
        red2 = Camera.main.WorldToScreenPoint(red3.position);
        player2 = Camera.main.WorldToScreenPoint(player3.position);

        var vecRed = red2 - player2;
        var vecBlk = black2 - player2;

        //var angleRed = Vector2.Angle(Vector2.right, vecRed);
        //var angleBlk = Vector2.Angle(Vector2.right, vecBlk);

        var angleRed = Vector2.SignedAngle(Vector2.right, vecRed);
        var angleBlk = Vector2.SignedAngle(Vector2.right, vecBlk);

        indicateRed.rotation = Quaternion.Euler(0f, 0f, angleRed);
        indicateBlk.rotation = Quaternion.Euler(0f, 0f, angleBlk);

        if(black2.x < 0 || black2.x > Screen.width || black2.y < 0 || black2.y > Screen.height)
        {
            indicateBlk.gameObject.SetActive(true);
        }
        else
        {
            indicateBlk.gameObject.SetActive(false);
        }

        if (red2.x < 0 || red2.x > Screen.width || red2.y < 0 || red2.y > Screen.height)
        {
            indicateRed.gameObject.SetActive(true);
        }
        else
        {
            indicateRed.gameObject.SetActive(false);
        }

        var viewRed = Camera.main.ScreenToViewportPoint(red2);
        var viewBlk = Camera.main.ScreenToViewportPoint(black2);

        viewRed = Reposition(viewRed);
        viewBlk = Reposition(viewBlk);

        indicateRed.position = Camera.main.ViewportToScreenPoint(viewRed);
        indicateBlk.position = Camera.main.ViewportToScreenPoint(viewBlk);
    }

    private Vector2 Reposition(Vector2 pos)
    {
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        return pos;
    }
}
