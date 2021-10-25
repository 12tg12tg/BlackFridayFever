using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public CharacterStats stats;
    public void CheckGameOver()
    {
        //게임매니저에서 종료 점수에 도달했는지 확인
        //도달했다면 게임매니저의 상태를 End로 바꾸면서 현재 우승자 AI 보내기? → 카메라우승자로 이동 → 플레이어로 이동.
        if (GameManager.GM.IsEnd(stats))
        {
            //끝났다면 캐릭터 IDLE애니메이션(또는 추가 애니메이션)
            var AiScript = GetComponentInParent<AiBehaviour>();
            if (AiScript == null) //플레이어
            {

            }
            else
            {
                AiScript.State = AIState.Idle;
            }
        }
        else
        {
            var AiScript = GetComponentInParent<AiBehaviour>();
            if (AiScript == null) //플레이어
            {

            }
            else
            {
                AiScript.State = AIState.FindMoney;
            }
        }
    }
}
