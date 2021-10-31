using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public CharacterStats stats;
    public void CheckGameOver() //�ִϸ��̼��±״��̰�;
    {
        var AiScript = GetComponentInParent<AiBehaviour>();
        if (AiScript != null)
        {
            AiScript.State = AIState.FindMoney;
        }
    }
}
