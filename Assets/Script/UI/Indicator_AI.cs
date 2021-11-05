using UnityEngine;
using UnityEngine.UI;

public class Indicator_AI : MonoBehaviour
{
    public Transform player;
    public Transform myObject;

    private Color color;

    public RectTransform trans;
    public RectTransform progressTrans;
    public RectTransform characterPicTrans;
    public RectTransform characterPrefabTrans;
    public Image indicateImg;
    public Image progressImg;
    public Image characterImg;
    public Image characterPrefabImg;

    public Color safeColor;
    public Color warnColor;

    public void Init(AiBehaviour ai)
    {
        myObject = ai.transform;
        var behavior =  ai as UnitBehaviour;
        indicateImg.color = behavior.meshColor;
        characterPrefabImg.sprite = myObject.GetComponentInChildren<PrefabImage>().resource;
    }

    private void Update()
    {
        if (GameManager.GM.State == GameManager.GameState.Play)
        {
            IndicateUpdate();
            ProgressUpdate();
        }
        else
        {
            ImageEnable(false);
        }
    }

    private void ProgressUpdate()
    {
        //비율
        var stats = myObject.GetComponent<CharacterStats>();

        var total = GameManager.GM.curStageInfo.goalScore;
        var cur = stats.truck.currentScore
            + stats.score;

        var ratio = cur / (float)total;
        ratio = Mathf.Clamp01(ratio);

        progressImg.fillAmount = ratio;

        //회전
        progressTrans.rotation = Quaternion.identity;
        characterPrefabTrans.rotation = Quaternion.identity;

        //색
        progressImg.color = Color.Lerp(safeColor, warnColor, ratio);
    }

    private void IndicateUpdate()
    {
        var player2D = Camera.main.WorldToScreenPoint(player.position);
        var myObject2D = Camera.main.WorldToScreenPoint(myObject.position);

        var vec = myObject2D - player2D;
        var angle = Vector2.SignedAngle(Vector2.right, vec);

        trans.rotation = Quaternion.Euler(0f, 0f, angle);

        if (myObject2D.x < 0 || myObject2D.x > Screen.width ||
            myObject2D.y < 0 || myObject2D.y > Screen.height)
        {
            ImageEnable(true);
        }
        else
        {
            ImageEnable(false);
        }

        var viewport = Camera.main.ScreenToViewportPoint(myObject2D);
        viewport = Reposition(viewport);

        //등지고있는경우
        var camPos = Camera.main.transform.position;
        Vector3 vec1 = Camera.main.transform.forward;
        Vector3 vec2 = (myObject.position - camPos).normalized;

        float dot = Vector3.Dot(vec1, vec2);

        if (dot < 0) //카메라가 등지고있는경우
        {
            viewport.x = 1 - viewport.x;
            viewport.y = 1 - viewport.y;
            trans.rotation *= Quaternion.Euler(0f, 0f, 180f);
        }

        trans.position = Camera.main.ViewportToScreenPoint(viewport);
    }

    private Vector2 Reposition(Vector2 pos)
    {
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        return pos;
    }

    private void ImageEnable(bool enable)
    {
        indicateImg.enabled = enable;
        progressImg.enabled = enable;
        characterImg.enabled = enable;
        characterPrefabImg.enabled = enable;
    }
}
