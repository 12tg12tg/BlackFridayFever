using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MultiTouch multiTouch;
    public CharacterStats stats;
    public Animator animator;
    public float stunTimer;
    private Vector3 direction;

    public void Init()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
        transform.position = stats.truck.dokingSpot.position + transform.forward * 3f;
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = stats.truck.bodyColor;
    }
    //private void Start()
    //{
    //    stats = GetComponent<CharacterStats>();
    //    animator = GetComponentInChildren<Animator>();
    //    transform.position = stats.truck.dokingSpot.position + transform.forward * 3f;
    //}
    private void Update()
    {
        switch (GameManager.GM.State)
        {
            case GameManager.GameState.Idle:
                break;
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Play:

                if (!stats.isStuned && !animator.GetCurrentAnimatorStateInfo(0).IsName("Push"))
                    Move();
                else
                {
                    stunTimer += Time.deltaTime;
                    if (stunTimer > stats.stats.stunTime)
                    {
                        stunTimer = 0f;
                        stats.isStuned = false;
                    }
                }
                AnimationUpdate();
                break;
            case GameManager.GameState.End:
                break;
        }
    }
    public void Move()
    {
        //이동 및 회전
        var joystick = multiTouch.Joystick;
        direction = new Vector3(joystick.x, 0, joystick.y);

        if (direction != Vector3.zero)
        {
            direction = Vector3.Lerp(transform.forward, direction, 8.0f * Time.deltaTime);
            transform.position += direction * stats.speed * Time.deltaTime;
            //var lerp = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    public void AnimationUpdate()
    {
        //애니
        if (direction == Vector3.zero)
            animator.SetFloat("Speed", 0f);
        else
            animator.SetFloat("Speed", 1f);

        animator.SetInteger("Stack", stats.itemStack);
    }
    public void CrushInit()
    {
        animator.SetTrigger("Stumble");
    }
    public void SetWinAnimation()
    {
        animator.SetTrigger("Dance");
    }
    public void SetDeafeatAnimation()
    {
        animator.SetTrigger("Defeated");
    }
}
