using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkingIndicator : MonoBehaviour
{
    public Transform playerTruck;
    public Transform player;
    public CharacterStats playerStats;

    public Image img;
    public RectTransform trans;

    public void Init()
    {
        playerTruck = player.GetComponent<CharacterStats>().truck.transform;
    }

    private void Update()
    {
        if (GameManager.GM.State == GameManager.GameState.Play)
        {
            IndicateUpdate();
        }
        else
        {
            ImageEnable(false);
        }
    }

    private void IndicateUpdate()
    {
        var player2D = Camera.main.WorldToScreenPoint(player.position);
        var myObject2D = Camera.main.WorldToScreenPoint(playerTruck.position);

        var vec = myObject2D - player2D;
        var angle = Vector2.SignedAngle(Vector2.right, vec);

        trans.rotation = Quaternion.Euler(0f, 0f, angle);

        if (player.position.z < -28f && Vector3.Distance(player.position, playerTruck.position) > 1f
            && playerStats.itemStack > 0)
        {
            ImageEnable(true);
        }
        else
        {
            ImageEnable(false);
        }

        //등지고있는경우
        var camPos = Camera.main.transform.position;
        Vector3 vec1 = Camera.main.transform.forward;
        Vector3 vec2 = (playerTruck.position - camPos).normalized;

        float dot = Vector3.Dot(vec1, vec2);

        if (dot < 0) //카메라가 등지고있는경우
        {
            trans.rotation *= Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private Vector2 Reposition(Vector2 pos)
    {
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        return pos;
    }

    private void ImageEnable(bool enable)
    {
        img.enabled = enable;
    }
}
