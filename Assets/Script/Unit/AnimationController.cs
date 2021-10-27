using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public CharacterStats stats;
    public void CheckGameOver() //애니메이션태그다이거;
    {
        var AiScript = GetComponentInParent<AiBehaviour>();
        if (AiScript != null)
        {
            AiScript.State = AIState.FindMoney;
        }
    }
}
