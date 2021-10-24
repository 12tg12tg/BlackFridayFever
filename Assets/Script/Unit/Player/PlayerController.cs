using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MultiTouch multiTouch;
    public CharacterStats stats;
    public Animator animator;
    public float stunTimer;

    private void Start()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponentInChildren<Animator>();
        transform.position = stats.truck.dokingSpot.position + transform.forward * 3f;
    }
    private void Update()
    {
        if(!stats.isStuned || !animator.GetCurrentAnimatorStateInfo(0).IsName("Push"))
            Move();
        else
        {
            stunTimer += Time.deltaTime;
            if(stunTimer > stats.stats.stunTime)
            {
                stunTimer = 0f;
                stats.isStuned = false;
            }
        }

    }
    public void Move()
    {
        //�̵� �� ȸ��
        var joystick = multiTouch.Joystick;
        var direction = new Vector3(joystick.x, 0, joystick.y);

        if (direction != Vector3.zero)
        {
            direction = Vector3.Lerp(transform.forward, direction, 5.0f * Time.deltaTime);
            transform.position += direction * stats.speed * Time.deltaTime;
            //var lerp = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        //�ִ�
        if(direction == Vector3.zero)
        {
            if(stats.itemStack == 0)
                animator.SetTrigger("JustIdle");
            else
                animator.SetTrigger("Lift");
        }
        else
        {
            if (stats.itemStack == 0)
                animator.SetTrigger("JustRun");
            else
                animator.SetTrigger("LiftRun");
        }
    }
    public void CrushInit()
    {
        animator.SetTrigger("Stumble");
    }
}
