using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public MultiTouch multiTouch;
    private void Update()
    {
        if (multiTouch.IsTap)
        {
            Debug.Log("IsTap");
        }
        if (multiTouch.IsDoubleTap)
        {
            Debug.Log("IsDoubleTap");
        }
        if (multiTouch.IsLongTap)
        {
            Debug.Log("IsLongTap");
        }
        var swipe = multiTouch.Swipe;
        if (swipe.x != 0 && swipe.y != 0)
        {
            if (swipe.x > 0)
                Debug.Log("Swipe Right");
            else
                Debug.Log("Swipe Left");
            if (swipe.y > 0)
                Debug.Log("Swipe Up");
            else
                Debug.Log("Swipe Down");
        }
        if (multiTouch.RotateAngle != 0f)
        {
            transform.Rotate(0f, 0f, multiTouch.RotateAngle);
        }
    }
}
