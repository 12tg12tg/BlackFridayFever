using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitBehaviour
{
    //��ġ�Է�
    public MultiTouch multiTouch;

    //���Ͻ� �̵������� �ɱ� ����.
    public float stunTimer;
    private Vector3 direction;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void Init()
    {
        transform.position = stats.truck.dokingSpot.position + transform.forward * 3f;
        GetComponentInChildren<SkinnedMeshRenderer>().material.color = stats.truck.bodyColor;
    }

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
        //�̵� �� ȸ��
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
        //�ִ�
        if (direction == Vector3.zero)
            setIdleAnimation();
        else
            setMoveAnimation();

        animator.SetInteger("Stack", stats.itemStack);
    }
}
