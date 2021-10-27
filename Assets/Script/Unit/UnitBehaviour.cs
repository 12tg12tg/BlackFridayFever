using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{
    public Animator animator;
    public CharacterStats stats;
    public void CrushInit() {
        animator.SetTrigger("Stumble");
    }
    public virtual void Init() { }
    public void setMoveAnimation() {
        animator.SetFloat("Speed", 1f);
    }
    public void setIdleAnimation() {
        animator.SetFloat("Speed", 0f);
    }
    public void SetWinAnimation(){
        animator.SetTrigger("Dance");
    }

    public void SetDeafeatAnimation(){
        animator.SetTrigger("Defeated");
    }
}
