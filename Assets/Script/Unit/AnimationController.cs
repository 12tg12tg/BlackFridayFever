using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public CharacterStats stats;
    public void CheckGameOver()
    {
        //���ӸŴ������� ���� ������ �����ߴ��� Ȯ��
        //�����ߴٸ� ���ӸŴ����� ���¸� End�� �ٲٸ鼭 ���� ����� AI ������? �� ī�޶����ڷ� �̵� �� �÷��̾�� �̵�.
        if (GameManager.GM.IsEnd(stats))
        {
            //�����ٸ� ĳ���� IDLE�ִϸ��̼�(�Ǵ� �߰� �ִϸ��̼�)
            var AiScript = GetComponentInParent<AiBehaviour>();
            if (AiScript == null) //�÷��̾�
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
            if (AiScript == null) //�÷��̾�
            {

            }
            else
            {
                AiScript.State = AIState.FindMoney;
            }
        }
    }
}
