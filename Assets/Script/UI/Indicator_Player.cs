using UnityEngine;
using UnityEngine.UI;

public class Indicator_Player : MonoBehaviour
{
    public Transform player;

    private Color color;

    public Image backgroundImg;
    public Image progressImg;
    public RectTransform progressTrans;


    public Color safeColor;
    public Color warnColor;

    private void Update()
    {
        if (GameManager.GM.State == GameManager.GameState.Play)
        {
            ProgressUpdate();
            ImageEnable(true);
        }
        else
        {
            ImageEnable(false);
        }
    }

    private void ProgressUpdate()
    {
        //비율
        var stats = player.GetComponent<CharacterStats>();

        var total = GameManager.GM.curStageInfo.goalScore;
        var cur = stats.truck.currentScore
            + stats.score;

        var ratio = cur / (float)total;
        ratio = Mathf.Clamp01(ratio);

        progressImg.fillAmount = ratio;

        //회전
        progressTrans.rotation = Quaternion.identity;

        //색
        progressImg.color = Color.Lerp(safeColor, warnColor, ratio);
    }

    private Vector2 Reposition(Vector2 pos)
    {
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        return pos;
    }

    private void ImageEnable(bool enable)
    {
        progressImg.enabled = enable;
        backgroundImg.enabled = enable;
    }
}
