using UnityEngine;
using UnityEngine.EventSystems;

public class MultiTouch : MonoBehaviour
{
    public float longTapDuration = 1f;

    // Swipe
    public float minSwipeDistanceInch = 0.25f; // Inch
    private float minSwipeDistancePixels;
    public float maxSwipeTime = 1f;

    private int fingerId = int.MinValue;
    private Vector2 touchStartPos;
    private float touchStartTime;

    // Zoom
    public float minZoomInch = 0.2f;
    public float maxZoomInch = 0.5f;

    // JoyStick
    private float minMoveInch = 0.1f;
    private float minMovePixels;

    public Vector2 TapPosition
    {
        get;
        private set;
    }

    public bool IsTap
    {
        get;
        private set;
    }
    public bool IsDoubleTap
    {
        get;
        private set;
    }
    public bool IsLongTap
    {
        get;
        private set;
    }

    public Vector2 Swipe
    {
        get;
        private set;
    }

    public Vector2 Joystick
    {
        get;
        private set;
    }

    public float Zoom
    {
        get;
        private set;
    }

    public float RotateAngle
    {
        get;
        private set;
    }

    private void Awake()
    {
        minSwipeDistancePixels = Screen.dpi * minSwipeDistanceInch; //픽셀계산.
        minMovePixels = Screen.dpi * minMoveInch;
    }


    Vector3 pos1;
    Vector3 pos2;
    Vector3 dir;
    float dis;
    private void Update()
    {
        switch (Input.touchCount)
        {
            case 0:
                IsTap = false;
                IsDoubleTap = false;
                IsLongTap = false;
                TapPosition = Vector2.zero;
                Zoom = 0f;
                Swipe = Vector2.zero;
                RotateAngle = 0f;
                Joystick = Vector2.zero;
                break;
            case 1:
                UpdateSingleTouch();
                break;
            case 2:
                UpdateDoubleTouch();
                break;
        }
    }

    private void UpdateSingleTouch()
    {
        var touch = Input.touches[0];
        switch (touch.phase)
        {
            case TouchPhase.Began:
                IsTap = true;
                TapPosition = touch.position;
                touchStartTime = Time.time;
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (Time.time > touchStartTime + longTapDuration)
                {
                    // Long
                    IsLongTap = true;
                    TapPosition = touch.position;
                }
                else if (touch.tapCount == 2)
                {
                    IsDoubleTap = true;
                    TapPosition = touch.position;
                }
                break;
        }

        //Swipe
        for (var i = 0; i < Input.touchCount; ++i)
        {
            touch = Input.touches[i]; //모든 터치정보 가져오기
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (fingerId == int.MinValue) //저장된 ID가 없었다면, 현재 ID를 저장하고 이것만 조사.
                    {
                        fingerId = touch.fingerId;
                        touchStartPos = touch.position; //시작위치
                        touchStartTime = Time.time; //시작시간
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (fingerId == touch.fingerId)
                    {
                        var endPos = touch.position;
                        var delta = endPos - touchStartPos;    //시작과 끝의 차이(변화량)
                        if (Mathf.Abs(delta.x) > minSwipeDistancePixels && Time.time < maxSwipeTime + touchStartTime)
                        {
                            Swipe = delta;
                        }
                        fingerId = int.MinValue;
                    }
                    break;
            }
        }

        //Joystick
        if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            var prevPos = touch.position - touch.deltaPosition;
            var dir = touch.position - prevPos;
            if (dir.magnitude > minMovePixels)
            {
                dir = dir.normalized;
                Joystick = dir;
            }
        }
        else
        {
            Joystick = Vector2.zero;
        }
    }

    private void UpdateDoubleTouch()
    {
        var touch0 = Input.touches[0];
        var touch1 = Input.touches[1];

        if (!EventSystem.current.IsPointerOverGameObject(touch0.fingerId) &&
            !EventSystem.current.IsPointerOverGameObject(touch1.fingerId))
        {
            // Pinch / Zoom
            var touch0PrevPos = touch0.position - touch0.deltaPosition; //이전프레임에서의 위치
            var touch1PrevPos = touch1.position - touch1.deltaPosition; //이전프레임에서의 위치

            var diffPrev = Vector2.Distance(touch0PrevPos, touch1PrevPos);
            var diffCurr = Vector2.Distance(touch0.position, touch1.position);

            var diffPixels = diffCurr - diffPrev; //+ : 확대 / - : 축소
            var diffInch = diffPixels / Screen.dpi;

            diffInch = Mathf.Clamp(diffInch, -minZoomInch, maxZoomInch);
            var scale = diffInch * Time.deltaTime;
            Zoom = diffInch;


            // Rotate
            if (touch0.phase != TouchPhase.Began && touch1.phase != TouchPhase.Began)
            {
                var prevDir = touch1PrevPos - touch0PrevPos;
                var currDir = touch1.position - touch0.position;

                var prevDegree = Vector3.SignedAngle(Vector3.up, prevDir, -Vector3.forward);
                var currDegree = Vector3.SignedAngle(Vector3.up, currDir, -Vector3.forward);


                RotateAngle = currDegree - prevDegree;
            }
        }
    }
}